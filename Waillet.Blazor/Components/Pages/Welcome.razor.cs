using Microsoft.AspNetCore.Components;

namespace Waillet.Blazor.Components.Pages;

public partial class Welcome
{
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private bool isUserMenuOpen;

    private void HandleCreateWallet()
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
