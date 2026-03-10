using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Waillet.Blazor.Models.Auth;
using Waillet.Blazor.Models.Forms;
using Waillet.Blazor.Services.Auth;

namespace Waillet.Blazor.Components.Pages;

public partial class Login
{
    [Inject] private IAuthApiClient AuthApiClient { get; set; } = default!;
    [Inject] private IAuthStateService AuthStateService { get; set; } = default!;
    [Inject] private IToastService ToastService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private readonly RegisterFormModel registerModel = new();
    private readonly LoginFormModel loginModel = new();

    private bool isBusy;
    private string activeTabId = "signin";

    private async Task HandleRegisterAsync()
    {
        if (isBusy)
        {
            return;
        }

        isBusy = true;

        try
        {
            var request = new RegisterUserRequest
            {
                Email = registerModel.Email,
                Password = registerModel.Password
            };

            var result = await AuthApiClient.RegisterAsync(request);
            if (!result.IsSuccess)
            {
                ToastService.ShowError(result.ErrorMessage ?? "Registration failed.");
                return;
            }

            AuthStateService.SetRegisteredUser(result.Value!);
            activeTabId = "signin";
            loginModel.Email = registerModel.Email;
            registerModel.Password = string.Empty;
            ToastService.ShowSuccess("Account created. Sign in to continue.");
        }
        finally
        {
            isBusy = false;
        }
    }

    private async Task HandleLoginAsync()
    {
        if (isBusy)
        {
            return;
        }

        isBusy = true;

        try
        {
            var request = new UserLoginRequest
            {
                UserName = loginModel.Email,
                Password = loginModel.Password
            };

            var result = await AuthApiClient.LoginAsync(request);
            if (!result.IsSuccess)
            {
                ToastService.ShowError(result.ErrorMessage ?? "Sign in failed.");
                return;
            }

            AuthStateService.SetLoginResult(result.Value!);
            NavigationManager.NavigateTo("/welcome");
        }
        finally
        {
            isBusy = false;
        }
    }

}
