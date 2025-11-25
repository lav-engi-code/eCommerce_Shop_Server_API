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

            string query = @"INSERT INTO Address (Name, Mobile, FullAddress, City, State, PinCode)
                         VALUES (@Name, @Mobile, @FullAddress, @City, @State, @PinCode)";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);
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

        public async Task<List<Address>> GetAllAddressMethod()
        {
            List<Address> addresses = new();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Address", sqlConnection);
            DataTable dt = new();
            da.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                addresses.Add(new Address
                {
                    Id = Convert.ToInt32(row["Id"]),
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

    }
}
