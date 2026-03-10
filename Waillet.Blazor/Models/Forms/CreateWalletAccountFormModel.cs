using System.ComponentModel.DataAnnotations;

namespace Waillet.Blazor.Models.Forms;

public class CreateWalletAccountFormModel
{
    public bool CryptoFlag { get; set; }

    [Required]
    [StringLength(10, ErrorMessage = "Currency code is too long")]
    public string CurrencyCode { get; set; } = string.Empty;
}
