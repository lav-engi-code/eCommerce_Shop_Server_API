using System.ComponentModel.DataAnnotations;

namespace eCommerce_Shop_Server_API.Modals
{
    public class Address
    {
        public int Id { get; set; }
        public string? UserEmail { get; set; }
        public string? Name { get; set; }
        public string? Mobile { get; set; }
        public string? FullAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PinCode { get; set; }
    }
}
