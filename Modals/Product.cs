using System.ComponentModel.DataAnnotations;

namespace eCommerce_Shop_Server_API.Modals
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Base64Img { get; set; }
        public int Quantity { get; set; }
    }
}
