using System.ComponentModel.DataAnnotations;

namespace WailletAPI.Dto;

public class UserLoginRequest
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    public required string UserName { get; set; }
    
    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public required string Password { get; set; }
}