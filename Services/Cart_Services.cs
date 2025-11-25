using eCommerce_Shop_Server_API.Modals;
using Microsoft.Data.SqlClient;
using System.Data;

namespace eCommerce_Shop_Server_API.Services
{
    public class Cart_Services
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection sqlConnection;

        public Cart_Services(IConfiguration configuration)
        {
            _configuration = configuration;
            this.sqlConnection = new SqlConnection(_configuration.GetConnectionString("Fast_Food").ToString());
        }


        public async Task<List<Product>> GetAllCartItems()
        {
            SqlDataAdapter sq = new SqlDataAdapter("SELECT * FROM Cart", sqlConnection);
            DataTable dt = new DataTable();
            sq.Fill(dt);

            List<Product> list = new();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Product
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = Convert.ToString(row["Name"]),
                    Description = Convert.ToString(row["Description"]),
                    Price = Convert.ToDecimal(row["Price"]),
                    Base64Img = Convert.ToString(row["Base64Img"]),
                    Quantity = Convert.ToInt32(row["Quantity"])
                });
            }

            return list;
        }


        public async Task<List<Product>> GetCartByIdMethod(int id)
        {
            SqlDataAdapter sq = new SqlDataAdapter($"SELECT * FROM Cart WHERE Id = {id}", sqlConnection);
            DataTable dataTable = new DataTable();
            sq.Fill(dataTable);
            List<Product> user1 = new List<Product>();
            foreach (DataRow row in dataTable.Rows)
            {
                user1.Add(new Product
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = Convert.ToString(row["Name"]),
                    Description = Convert.ToString(row["Description"]),
                    Price = Convert.ToDecimal(row["Price"]),
                    Base64Img = Convert.ToString(row["Base64Img"]),
                    Quantity = Convert.ToInt32(row["Quantity"]),
                });
            }
            return user1;
        }
        public IResult AddCartMethod(Product Ed)
        {
            sqlConnection.Open();

            // 1️⃣ Check if product already exists in cart
            string check = "SELECT Id, Quantity FROM Cart WHERE Name = @Name";
            SqlCommand checkCmd = new SqlCommand(check, sqlConnection);
            checkCmd.Parameters.AddWithValue("@Name", Ed.Name);

            SqlDataReader reader = checkCmd.ExecuteReader();

            if (reader.Read())
            {
                // Product already exists → Update quantity
                int id = Convert.ToInt32(reader["Id"]);
                int qty = Convert.ToInt32(reader["Quantity"]);
                reader.Close();

                string update = "UPDATE Cart SET Quantity = @Qty WHERE Id = @Id";
                SqlCommand updateCmd = new SqlCommand(update, sqlConnection);
                updateCmd.Parameters.AddWithValue("@Qty", qty + 1);
                updateCmd.Parameters.AddWithValue("@Id", id);

                updateCmd.ExecuteNonQuery();
            }
            else
            {
                reader.Close();

                // Product does not exist → Insert new row
                string insert = @"
                    INSERT INTO Cart (Name, Description, Price, Base64Img, Quantity)
                    VALUES (@Name, @Description, @Price, @Base64Img, @Quantity)";

                SqlCommand insertCmd = new SqlCommand(insert, sqlConnection);
                insertCmd.Parameters.AddWithValue("@Name", Ed.Name);
                insertCmd.Parameters.AddWithValue("@Description", Ed.Description);
                insertCmd.Parameters.AddWithValue("@Price", Ed.Price);
                insertCmd.Parameters.AddWithValue("@Base64Img", Ed.Base64Img);
                insertCmd.Parameters.AddWithValue("@Quantity", 1);

                insertCmd.ExecuteNonQuery();
            }

            sqlConnection.Close();
            return Results.Ok("Cart updated!");
        }

        public IResult UpdateCartMethod(Product Ed)
        {
            sqlConnection.Open();
            string quar = "UPDATE Cart SET " +"Name = '" + Ed.Name + "', " + "Description = '" + Ed.Description + "', " + "Price = " + Ed.Price + ", " + "Base64Img = '" + Ed.Base64Img + "', " +"Quantity = " + Ed.Quantity +" WHERE Id = " + Ed.Id;
            SqlCommand sqlCommand = new SqlCommand(quar, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            int r = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return r > 0 ? Results.Ok("Updated successfully") : Results.NotFound("Item not found");
        }

        public IResult UpdateCartQtyById(int id, int qty)
        {
            sqlConnection.Open();

            string quar = $"UPDATE Cart SET Quantity = {qty} WHERE Id = {id}";

            SqlCommand cmd = new SqlCommand(quar, sqlConnection);
            int r = cmd.ExecuteNonQuery();

            sqlConnection.Close();
            return r > 0 ? Results.Ok("Quantity Updated") : Results.NotFound("Item not found");
        }

        public IResult DeleteCartMethod(int id)
        {
            sqlConnection.Open();
            string quar = "DELETE FROM Cart WHERE Id = " + id;
            SqlCommand sqlCommand = new SqlCommand(quar, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            int r = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return r > 0 ? Results.Ok("Deleted successfully") : Results.NotFound("Item not found");
        }
    }
}
