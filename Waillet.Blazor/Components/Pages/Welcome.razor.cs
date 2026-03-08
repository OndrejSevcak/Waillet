using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Waillet.Blazor.Models.Forms;

namespace Waillet.Blazor.Components.Pages;

public partial class Welcome
{
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private readonly CreateWalletAccountFormModel createAccountModel = new();
    private readonly IReadOnlyList<bool> accountTypeOptions = new[] { true, false };
    //THIS SHOULD BE REPLACED WITH API CALL TO GET SUPPORTED CURRENCIES
    private readonly IReadOnlyList<string> currencyOptions = new[] { "EUR", "USD", "BTC", "ETH" };

    private bool isUserMenuOpen;
    private bool showWalletForm;

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
