using Microsoft.Data.SqlClient;
using System.Data;

namespace eCommerce_Shop_Server_API.Modals
{
    public class Archive_Contact
    {
        public DateTime? Date { get; set; }
        public string? Full_Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Message { get; set; }
    }
}
