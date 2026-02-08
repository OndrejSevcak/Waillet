using System.ComponentModel.DataAnnotations;

namespace WailletAPI.Dto
{
    public class CreateTransactionRequest
    {
        [Required]
        public long FromAccKey { get; set; }

        [Required]
        public long ToAccKey { get; set; }

        [Required]
        [Range(0.00000001, double.MaxValue)]
        public decimal Amount { get; set; }

        // Optional: type for future expansion
        public string? Type { get; set; }
    }
}
