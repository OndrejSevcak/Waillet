using System.ComponentModel.DataAnnotations;

namespace Waillet.Blazor.Models.Forms;

public class CreateWalletAccountFormModel
{
    public bool CryptoFlag { get; set; }

    [Required]
    [StringLength(10, ErrorMessage = "Currency code is too long")]
    public string CurrencyCode { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^\\d+(\\.\\d{1,2})?$", ErrorMessage = "Enter a valid amount")]
    public string InitialBalance { get; set; } = "0.00";
}
