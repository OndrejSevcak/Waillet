using Microsoft.AspNetCore.Components;
using Waillet.Blazor.Models.Forms;
using Waillet.Blazor.Services.Wallet;

namespace Waillet.Blazor.Components.Pages;

public partial class Welcome
{
    [Inject] private IWalletApiClient WalletApiClient { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private readonly CreateWalletAccountFormModel createAccountModel = new();
    private readonly IReadOnlyList<bool> accountTypeOptions = new[] { true, false };
    private IReadOnlyList<string> currencyOptions = Array.Empty<string>();

    private bool isUserMenuOpen;
    private bool showWalletForm;
    private bool isAssetsLoading;

    protected override async Task OnInitializedAsync()
    {
        isAssetsLoading = true;
        try
        {
            var result = await WalletApiClient.GetSupportedAssetsAsync();
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

                return;
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
    }

    private void HandleCreateWalletSubmit()
    {
        NavigationManager.NavigateTo("/wallet/new");
    }

    private void ToggleUserMenu()
    {
        isUserMenuOpen = !isUserMenuOpen;
    }

    private void HandleLogout()
    {
        isUserMenuOpen = false;
        NavigationManager.NavigateTo("/login");
    }
}
