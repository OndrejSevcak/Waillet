using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Waillet.Blazor.Models.Forms;
using Waillet.Blazor.Services.Wallet;

namespace Waillet.Blazor.Components.Pages;

[Authorize]
public partial class Welcome : IDisposable
{
    [Inject] private IWalletApiClient WalletApiClient { get; set; } = default!;
    [Inject] private IToastService ToastService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private readonly CreateWalletAccountFormModel createAccountModel = new();
    private readonly IReadOnlyList<bool> accountTypeOptions = new[] { true, false };
    private IReadOnlyList<string> currencyOptions = Array.Empty<string>();
    private readonly CancellationTokenSource _cts = new();

    private bool isUserMenuOpen;
    private bool showWalletForm;
    private bool isAssetsLoading;
    private bool isBusy;
    private string? assetsLoadError;

    protected override async Task OnInitializedAsync()
    {
        isAssetsLoading = true;
        try
        {
            var result = await WalletApiClient.GetSupportedAssetsAsync(_cts.Token);
            if (result.IsSuccess && result.Value is not null)
            {
                currencyOptions = result.Value
                    .OrderBy(asset => asset.Symbol, StringComparer.OrdinalIgnoreCase)
                    .Select(asset => asset.Symbol)
                    .ToList();

                if (string.IsNullOrWhiteSpace(createAccountModel.CurrencyCode) && currencyOptions.Count > 0)
                {
                    createAccountModel.CurrencyCode = currencyOptions[0];
                }
            }
            else
            {
                assetsLoadError = result.ErrorMessage ?? "Failed to load supported assets.";
            }
        }
        finally
        {
            isAssetsLoading = false;
        }
    }

    private void HandleCreateWallet()
    {
        showWalletForm = true;
    }

    private void HandleCancelCreateWallet()
    {
        showWalletForm = false;
        createAccountModel.CurrencyCode = currencyOptions.Count > 0 ? currencyOptions[0] : string.Empty;
        createAccountModel.CryptoFlag = false;
    }

    private async Task HandleCreateWalletSubmitAsync()
    {
        if (isBusy) return;

        isBusy = true;
        try
        {
            var result = await WalletApiClient.CreateWalletAccountAsync(
                createAccountModel.CurrencyCode, _cts.Token);

            if (!result.IsSuccess)
            {
                ToastService.ShowError(result.ErrorMessage ?? "Failed to create wallet account.");
                return;
            }

            ToastService.ShowSuccess($"Wallet account created for {createAccountModel.CurrencyCode}.");
            showWalletForm = false;
            createAccountModel.CurrencyCode = currencyOptions.Count > 0 ? currencyOptions[0] : string.Empty;
            createAccountModel.CryptoFlag = false;
        }
        finally
        {
            isBusy = false;
        }
    }

    private void ToggleUserMenu()
    {
        isUserMenuOpen = !isUserMenuOpen;
    }

    private void HandleDropdownKeyDown(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs e)
    {
        if (e.Key == "Escape") isUserMenuOpen = false;
    }

    private void HandleLogout()
    {
        isUserMenuOpen = false;
        NavigationManager.NavigateTo("/login");
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
