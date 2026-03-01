using WailletAPI.Common;
using WailletAPI.Dto;
using WailletAPI.Models.Auth;

namespace WailletAPI.Services;

public interface IAuthService
{
    Task<Result<UserDto>> RegisterUser(RegisterUserRequest req);
    Task<Result<LoginResponse>> LoginUser(UserLoginRequest req);
}