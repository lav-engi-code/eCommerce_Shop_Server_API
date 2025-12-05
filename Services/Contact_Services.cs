using eCommerce_Shop_Server_API.Modals;
using Microsoft.Data.SqlClient;
using System.Data;

namespace eCommerce_Shop_Server_API.Services
{
    public class Contact_Services
    {

        private readonly IConfiguration _configuration;
        private readonly SqlConnection sqlConnection;

        public Contact_Services(IConfiguration configuration)
        {
            _configuration = configuration;
            this.sqlConnection = new SqlConnection(_configuration.GetConnectionString("Fast_Food").ToString());
        }


        public async Task<List<Contact>> GetContactMethod()
        {
            SqlDataAdapter sq = new SqlDataAdapter("SELECT * FROM Contact", sqlConnection);
            DataTable dataTable = new DataTable();
            sq.Fill(dataTable);

            List<Contact> user1 = new List<Contact>();

            foreach (DataRow row in dataTable.Rows)
            {
                user1.Add(new Contact
                {
                    Full_Name = row["Full_Name"]?.ToString(),
                    Email = row["Email"]?.ToString(),
                    Phone = row["Phone"]?.ToString(),
                    Message = row["Message"]?.ToString(),
                    Date = ((DateTimeOffset)row["Date"]).DateTime

                });
            }

            return user1;
        }

        public IResult AddContactMethod(Contact Ed)
        {
            sqlConnection.Open();
            string quar = "INSERT INTO Contact(Full_Name, Email, Phone,Message) VALUES('" + Ed.Full_Name + "', '" + Ed.Email + "', '" + Ed.Phone + "', '" + Ed.Message + "')";
            SqlCommand sqlCommand = new SqlCommand(quar, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            int r = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return Results.Ok();
        }

        public IResult DeleteContactByPhone(string phone)
        {
            try
            {
                sqlConnection.Open();
                string query = "DELETE FROM Contact WHERE Phone = @Phone";
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Phone", phone);
                int rowsAffected = cmd.ExecuteNonQuery();
                sqlConnection.Close();
                if (rowsAffected > 0)
                    return Results.Ok(new { message = "Contact deleted successfully." });
                else
                    return Results.NotFound(new { message = "Contact not found." });
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                return Results.Problem(ex.Message);
            }
        }
        public IResult ArchiveThenDeleteContact(Contact contact)
        {
            try
            {
                Archive_Contact_Services archiveService = new Archive_Contact_Services(_configuration);
                Archive_Contact archive = new Archive_Contact
                {
                    Full_Name = contact.Full_Name,
                    Email = contact.Email,
                    Phone = contact.Phone,
                    Message = contact.Message
                };
                archiveService.AddArchiveContactMethod(archive);

                // 2. Delete the contact from Contact table
                DeleteContactByPhone(contact.Phone);

                return Results.Ok(new { message = "Contact archived and deleted successfully." });
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
