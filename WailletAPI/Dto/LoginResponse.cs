namespace WailletAPI.Dto;

public record LoginResponse(string AccessToken, string TokenType, int ExpiresIn);
