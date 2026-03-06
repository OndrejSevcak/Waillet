using Waillet.Blazor.Models.Auth;
using Waillet.Blazor.Services;

namespace Waillet.Blazor.Services.Auth;

public interface IAuthApiClient
{
    Task<ApiResult<UserDto>> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken = default);
    Task<ApiResult<LoginResponse>> LoginAsync(UserLoginRequest request, CancellationToken cancellationToken = default);
}
