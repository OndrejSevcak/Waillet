using WailletAPI.Dto;
using WailletAPI.Models.Auth;

namespace WailletAPI.Services;

public interface IAuthService
{
    Task<AuthResult<UserDto>> RegisterUser(RegisterUserRequest req);
    Task<AuthResult<LoginResponse>> LoginUser(UserLoginRequest req);
}