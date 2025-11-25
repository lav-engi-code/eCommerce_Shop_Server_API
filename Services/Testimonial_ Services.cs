using eCommerce_Shop_Server_API.Modals;
using Microsoft.Data.SqlClient;
using System.Data;

namespace eCommerce_Shop_Server_API.Services
{
    public class Testimonial__Services
    {

        private readonly IConfiguration _configuration;
        private readonly SqlConnection sqlConnection;

        public Testimonial__Services(IConfiguration configuration)
        {
            _configuration = configuration;
            this.sqlConnection = new SqlConnection(_configuration.GetConnectionString("Fast_Food").ToString());
        }

        public async Task<List<Testimonial>> GetReviewMethod()
        {
            SqlDataAdapter sq = new SqlDataAdapter("SELECT * FROM Testimonial", sqlConnection);
            DataTable dataTable = new DataTable();
            sq.Fill(dataTable);
            List<Testimonial> user1 = new List<Testimonial>();
            foreach (DataRow row in dataTable.Rows)
            {
                user1.Add(new Testimonial
                {
                    Reviewer_Name = Convert.ToString(row["Reviewer_Name"]),
                    Review_Text = Convert.ToString(row["Review_Text"]),
                    Star = Convert.ToInt32(row["Star"])
                });
            }
            return user1;
        }


        public IResult AddReviewMethod(Testimonial Ed)
        {
            sqlConnection.Open();
            string quar = "INSERT INTO Testimonial(Reviewer_Name, Review_Text, Star) VALUES('" + Ed.Reviewer_Name + "', '" + Ed.Review_Text + "', " + Ed.Star + ")";
            SqlCommand sqlCommand = new SqlCommand(quar, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            int r = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return Results.Ok();
        }

        public IResult UpdateReviewMethodByName(Testimonial Ed)
        {
            sqlConnection.Open();

            string quar = "UPDATE Testimonial SET " +
                          "Review_Text = '" + Ed.Review_Text + "', " +
                          "Star = " + Ed.Star +
                          " WHERE Reviewer_Name = '" + Ed.Reviewer_Name + "'";

            SqlCommand sqlCommand = new SqlCommand(quar, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            int r = sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();
            return Results.Ok();
        }

        public IResult DeleteReviewByName(string reviewerName)
        {
            sqlConnection.Open();
            string quar = "DELETE FROM Testimonial WHERE Reviewer_Name = '" + reviewerName + "'";
            SqlCommand sqlCommand = new SqlCommand(quar, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            int r = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return Results.Ok();
        }
    }
}
