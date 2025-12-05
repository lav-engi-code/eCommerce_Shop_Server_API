using eCommerce_Shop_Server_API.Modals;
using Microsoft.Data.SqlClient;
using System.Data;

namespace eCommerce_Shop_Server_API.Services
{
    public class Archive_Contact_Services
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection sqlConnection;

        public Archive_Contact_Services(IConfiguration configuration)
        {
            _configuration = configuration;
            this.sqlConnection = new SqlConnection(_configuration.GetConnectionString("Fast_Food").ToString());
        }

        public IResult AddArchiveContactMethod(Archive_Contact Ed)
        {
            sqlConnection.Open(); 
            string quar = "INSERT INTO Archive_Contact(Full_Name, Email, Phone,Message) VALUES('" + Ed.Full_Name + "', '" + Ed.Email + "', '" + Ed.Phone + "', '" + Ed.Message + "')";
            SqlCommand sqlCommand = new SqlCommand(quar, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            int r = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return Results.Ok();
        }
    }
}
