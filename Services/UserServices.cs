using eCommerce_Shop_Server_API.Modals;
using Microsoft.Data.SqlClient;

namespace eCommerce_Shop_Server_API.Services
{
    public class UserServices
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection sqlConnection;

        public UserServices(IConfiguration configuration)
        {
            _configuration = configuration;
            this.sqlConnection = new SqlConnection(_configuration.GetConnectionString("Fast_Food").ToString());
        }


        public async Task<string> RegisterUser(RegisterModel model)
        {
            try
            {
                if (model.Password != model.ConfirmPassword)
                    return "Passwords do not match";

                await sqlConnection.OpenAsync();

                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";

                SqlCommand checkCmd = new SqlCommand(checkQuery, sqlConnection);
                checkCmd.Parameters.AddWithValue("@Email", model.Email);

                int exists = (int)await checkCmd.ExecuteScalarAsync();
                if (exists > 0)
                {
                    return "Email already registered";
                }

                string insertQuery = @"INSERT INTO Users (FullName, Email, Password)
                                   VALUES (@FullName, @Email, @Password)";

                SqlCommand cmd = new SqlCommand(insertQuery, sqlConnection);
                cmd.Parameters.AddWithValue("@FullName", model.FullName);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@Password", model.Password);

                await cmd.ExecuteNonQueryAsync();

                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }
        }


        public async Task<string> LoginUser(LoginModel model)
        {
            try
            {
                await sqlConnection.OpenAsync();

                string query = "SELECT FullName, Password FROM Users WHERE Email = @Email";

                using SqlCommand cmd = new SqlCommand(query, sqlConnection);
                cmd.Parameters.AddWithValue("@Email", model.Email);

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (!reader.Read())
                {
                    return "Invalid email";
                }

                string dbName = reader["FullName"].ToString();
                string dbPassword = reader["Password"].ToString();

                if (dbPassword != model.Password)
                {
                    return "Wrong password";
                }

                return $"ok|{dbName}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }
        }

    }
}
