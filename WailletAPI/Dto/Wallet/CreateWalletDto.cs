using System.ComponentModel.DataAnnotations;

namespace WailletAPI.Dto;

public class CreateWalletDto
{    
    [Required]
    public string Asset { get; set; }
}