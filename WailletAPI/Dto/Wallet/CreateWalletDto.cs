using System.ComponentModel.DataAnnotations;

namespace WailletAPI.Dto;

public class CreateWalletDto
{
    [Required]
    public long UserKey { get; set; }
    
    [Required]
    public string Asset { get; set; }
}