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
                // 1️⃣ Match password + confirm password
                if (model.Password != model.ConfirmPassword)
                    return "Passwords do not match";

                await sqlConnection.OpenAsync();

                // 2️⃣ Check if email already exists
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";

                SqlCommand checkCmd = new SqlCommand(checkQuery, sqlConnection);
                checkCmd.Parameters.AddWithValue("@Email", model.Email);

                int exists = (int)await checkCmd.ExecuteScalarAsync();
                if (exists > 0)
                {
                    return "Email already registered";
                }

                // 3️⃣ Insert user
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

                // 1️⃣ Check if email exists
                string query = "SELECT Password FROM Users WHERE Email = @Email";

                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                cmd.Parameters.AddWithValue("@Email", model.Email);

                object result = await cmd.ExecuteScalarAsync();

                if (result == null)
                {
                    return "Invalid email";
                }

                string storedPassword = result.ToString();

                // 2️⃣ Verify password
                if (storedPassword != model.Password)
                {
                    return "Wrong password";
                }

                return "ok"; // Login success
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
