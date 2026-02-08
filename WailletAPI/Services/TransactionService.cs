using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WailletAPI.Dto;
using WailletAPI.Models;
using WailletAPI.Repository;
using WailletAPI.Data;

namespace WailletAPI.Services;

public class TransactionService
{
    private readonly AccountRepository _accountRepo;
    private readonly TransactionRepository _txRepo;
    private readonly IExchangeRateService _rateService;
    private readonly WailletDbContext _dbContext;

    public TransactionService(AccountRepository accountRepo, TransactionRepository txRepo, IExchangeRateService rateService, WailletDbContext dbContext)
    {
        _accountRepo = accountRepo;
        _txRepo = txRepo;
        _rateService = rateService;
        _dbContext = dbContext;
    }

    public async Task<TransactionDto> ExchangeAsync(CreateTransactionRequest req, ClaimsPrincipal user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        // Extract user key from JWT 'sub' claim
        var sub = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
        if (!long.TryParse(sub, out var initiatorUserKey))
            throw new UnauthorizedAccessException("Invalid user token");

        // Load accounts
        var fromAcc = await _dbContext.Accounts.FindAsync(req.FromAccKey);
        var toAcc = await _dbContext.Accounts.FindAsync(req.ToAccKey);

        if (fromAcc == null || toAcc == null)
            throw new KeyNotFoundException("One or both accounts not found");

        // Ensure both accounts belong to the same user (no cross-user transfers)
        if (fromAcc.UserKey != toAcc.UserKey)
            throw new InvalidOperationException("Transfers between different users are not allowed");

        // Ensure initiator owns the from account
        if (fromAcc.UserKey != initiatorUserKey)
            throw new UnauthorizedAccessException("User does not own the source account");

        if (req.Amount <= 0)
            throw new InvalidOperationException("Amount must be greater than zero");

        if (fromAcc.Balance < req.Amount)
            throw new InvalidOperationException("Insufficient funds");

        // Determine currencies
        var currencyFrom = fromAcc.CurrencyCode ?? (fromAcc.CryptoFlag ? "" : "EUR");
        var currencyTo = toAcc.CurrencyCode ?? (toAcc.CryptoFlag ? "" : "EUR");

        // Get rate
        var rate = await _rateService.GetRateAsync(currencyFrom, currencyTo);
        if (rate <= 0)
            throw new InvalidOperationException("Invalid exchange rate");

        var amountTo = Math.Round(req.Amount * rate, 8);

        // Perform DB transaction
        await using var tx = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            // Update balances
            fromAcc.Balance -= req.Amount;
            toAcc.Balance += amountTo;

            _dbContext.Accounts.Update(fromAcc);
            _dbContext.Accounts.Update(toAcc);

            // Save accounts
            await _dbContext.SaveChangesAsync();

            // Create transaction record
            var record = new Transaction
            {
                FromAccKey = fromAcc.AccKey,
                ToAccKey = toAcc.AccKey,
                AmountFrom = req.Amount,
                AmountTo = amountTo,
                CurrencyFrom = currencyFrom,
                CurrencyTo = currencyTo,
                Rate = rate,
                Type = req.Type ?? "exchange",
                Status = "completed",
                InitiatorUserKey = initiatorUserKey,
                CreatedAt = DateTime.UtcNow
            };

            await _txRepo.AddTransactionAsync(record);

            await tx.CommitAsync();

            return new TransactionDto
            {
                TxKey = record.TxKey,
                FromAccKey = record.FromAccKey,
                ToAccKey = record.ToAccKey,
                AmountFrom = record.AmountFrom,
                AmountTo = record.AmountTo,
                Rate = record.Rate,
                CurrencyFrom = record.CurrencyFrom,
                CurrencyTo = record.CurrencyTo,
                Type = record.Type,
                Status = record.Status,
                InitiatorUserKey = record.InitiatorUserKey,
                CreatedAt = record.CreatedAt
            };
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }
}
