using System.ComponentModel.DataAnnotations;

namespace eCommerce_Shop_Server_API.Modals
{
    public class Address
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Mobile { get; set; }
        [Required]
        public string? FullAddress { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? State { get; set; }
        [Required]
        public string? PinCode { get; set; }

    }
}
