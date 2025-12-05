using eCommerce_Shop_Server_API.Modals;
using Microsoft.Data.SqlClient;
using System.Data;

namespace eCommerce_Shop_Server_API.Services
{
    public class Address_Services
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection sqlConnection;

        public Address_Services(IConfiguration configuration)
        {
            _configuration = configuration;
            this.sqlConnection = new SqlConnection(_configuration.GetConnectionString("Fast_Food").ToString());
        }


        public IResult AddAddressMethod(Address ad)
        {
            sqlConnection.Open();
            string query = @"INSERT INTO Address (UserEmail, Name, Mobile, FullAddress, City, State, PinCode)
                             VALUES (@UserEmail, @Name, @Mobile, @FullAddress, @City, @State, @PinCode)";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            cmd.Parameters.AddWithValue("@UserEmail", ad.UserEmail);
            cmd.Parameters.AddWithValue("@Name", ad.Name);
            cmd.Parameters.AddWithValue("@Mobile", ad.Mobile);
            cmd.Parameters.AddWithValue("@FullAddress", ad.FullAddress);
            cmd.Parameters.AddWithValue("@City", ad.City);
            cmd.Parameters.AddWithValue("@State", ad.State);
            cmd.Parameters.AddWithValue("@PinCode", ad.PinCode);

            cmd.ExecuteNonQuery();
            sqlConnection.Close();

            return Results.Ok(new { message = "Address saved successfully" });
        }

        // Get all addresses for a specific user
        public async Task<List<Address>> GetAddressesByUserEmail(string email)
        {
            List<Address> addresses = new();
            string query = "SELECT * FROM Address WHERE UserEmail = @UserEmail";

            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            da.SelectCommand.Parameters.AddWithValue("@UserEmail", email);

            DataTable dt = new();
            da.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                addresses.Add(new Address
                {
                    Id = Convert.ToInt32(row["Id"]),
                    UserEmail = Convert.ToString(row["UserEmail"]),
                    Name = Convert.ToString(row["Name"]),
                    Mobile = Convert.ToString(row["Mobile"]),
                    FullAddress = Convert.ToString(row["FullAddress"]),
                    City = Convert.ToString(row["City"]),
                    State = Convert.ToString(row["State"]),
                    PinCode = Convert.ToString(row["PinCode"])
                });
            }

            return addresses;
        }

        // Get a single address by ID
        public Address GetAddressById(int id)
        {
            sqlConnection.Open();
            string query = "SELECT * FROM Address WHERE Id = @Id";
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            cmd.Parameters.AddWithValue("@Id", id);

            SqlDataReader reader = cmd.ExecuteReader();
            Address address = null;

            if (reader.Read())
            {
                address = new Address
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    UserEmail = Convert.ToString(reader["UserEmail"]),
                    Name = Convert.ToString(reader["Name"]),
                    Mobile = Convert.ToString(reader["Mobile"]),
                    FullAddress = Convert.ToString(reader["FullAddress"]),
                    City = Convert.ToString(reader["City"]),
                    State = Convert.ToString(reader["State"]),
                    PinCode = Convert.ToString(reader["PinCode"])
                };
            }

            sqlConnection.Close();
            return address;
        }

        // Delete an address by ID
        public IResult DeleteAddress(int id)
        {
            sqlConnection.Open();
            string query = "DELETE FROM Address WHERE Id = @Id";
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
            sqlConnection.Close();

            return Results.Ok(new { message = "Address deleted successfully" });
        }

        // Update an address
        public IResult UpdateAddress(Address ad)
        {
            sqlConnection.Open();
            string query = @"UPDATE Address 
                             SET Name = @Name, Mobile = @Mobile, FullAddress = @FullAddress,
                                 City = @City, State = @State, PinCode = @PinCode
                             WHERE Id = @Id AND UserEmail = @UserEmail";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            cmd.Parameters.AddWithValue("@Id", ad.Id);
            cmd.Parameters.AddWithValue("@UserEmail", ad.UserEmail);
            cmd.Parameters.AddWithValue("@Name", ad.Name);
            cmd.Parameters.AddWithValue("@Mobile", ad.Mobile);
            cmd.Parameters.AddWithValue("@FullAddress", ad.FullAddress);
            cmd.Parameters.AddWithValue("@City", ad.City);
            cmd.Parameters.AddWithValue("@State", ad.State);
            cmd.Parameters.AddWithValue("@PinCode", ad.PinCode);

            cmd.ExecuteNonQuery();
            sqlConnection.Close();

            return Results.Ok(new { message = "Address updated successfully" });
        }
    }
}
