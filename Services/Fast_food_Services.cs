using eCommerce_Shop_Server_API.Modals;
using Microsoft.Data.SqlClient;
using System.Data;

namespace eCommerce_Shop_Server_API.Services
{
    public class Fast_food_Services
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection sqlConnection;

        public Fast_food_Services(IConfiguration configuration)
        {
            _configuration = configuration;
            this.sqlConnection = new SqlConnection(_configuration.GetConnectionString("Fast_Food").ToString());
        }

        public async Task<List<Product>> GetFoodMethod()
        {
            SqlDataAdapter sq = new SqlDataAdapter("SELECT * FROM Fast_Food", sqlConnection);
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
        public Product GetFoodByIdMethod(int id)
        {
            sqlConnection.Open();

            string query = "SELECT * FROM Fast_Food WHERE Id = @Id";
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            cmd.Parameters.AddWithValue("@Id", id);

            SqlDataReader reader = cmd.ExecuteReader();

            Product product = null;
            if (reader.Read())
            {
                product = new Product
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString(),
                    Description = reader["Description"].ToString(),
                    Price = (decimal)reader["Price"],
                    Quantity = (int)reader["Quantity"],
                    Base64Img = reader["Base64Img"].ToString()
                };
            }

            sqlConnection.Close();

            if (product == null)
                return null;

            return product;
        }

        public IResult UpdateFoodMethod(int id, Product updated)
        {
            sqlConnection.Open();
            string query = @"UPDATE Fast_Food 
                     SET Name=@Name, Description=@Description, Price=@Price, 
                         Base64Img=@Base64Img, Quantity=@Quantity 
                     WHERE Id=@Id";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            cmd.Parameters.AddWithValue("@Name", updated.Name);
            cmd.Parameters.AddWithValue("@Description", updated.Description);
            cmd.Parameters.AddWithValue("@Price", updated.Price);
            cmd.Parameters.AddWithValue("@Base64Img", updated.Base64Img ?? "");
            cmd.Parameters.AddWithValue("@Quantity", updated.Quantity);
            cmd.Parameters.AddWithValue("@Id", id);

            int rows = cmd.ExecuteNonQuery();
            sqlConnection.Close();

            return rows > 0 ? Results.Ok() : Results.NotFound();
        }


        public IResult AddFoodMethod(Product Ed)
        {
            sqlConnection.Open();
            string quar = "INSERT INTO Fast_Food(Name, Description, Price, Base64Img, Quantity) VALUES('" + Ed.Name + "', '" + Ed.Description + "', " + Ed.Price + ", '" + Ed.Base64Img + "', " + Ed.Quantity + ")";
            SqlCommand sqlCommand = new SqlCommand(quar, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            int r = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return Results.Ok();
        }

        public IResult DeleteFoodMethod(int id)
        {
            try
            {
                sqlConnection.Open();

                string query = "DELETE FROM Fast_Food WHERE Id = @Id";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@Id", id);

                int rowsAffected = sqlCommand.ExecuteNonQuery();

                sqlConnection.Close();

                if (rowsAffected > 0)
                    return Results.Ok(new { message = "Product deleted successfully." });
                else
                    return Results.NotFound(new { message = "Product not found." });
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                return Results.Problem(ex.Message);
            }
        }

    }
}
