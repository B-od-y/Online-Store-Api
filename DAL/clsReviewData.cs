using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class ReviewDTO
    {
        public int ReviewID { get; set; }
        public int ProductID { get; set; }
        public int CustomerID { get; set; }
        public float Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
        public ReviewDTO(int reviewID, int productID, int customerID, float rating, string reviewText, DateTime reviewDate)
        {
            ReviewID = reviewID;
            ProductID = productID;
            CustomerID = customerID;
            Rating = rating;
            ReviewText = reviewText;
            ReviewDate = reviewDate;
        }
    }
    public class clsReviewData
    {
        public static List<ReviewDTO> GetAllReviews()
        {
            List<ReviewDTO> ReviewDTo = new List<ReviewDTO>();
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllReviews", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ReviewDTo.Add(new ReviewDTO(
                                reader.GetInt32("ReviewID"),
                                reader.GetInt32("ProductID"),
                                reader.GetInt32("CustomerID"),
                                Convert.ToSingle(reader["Rating"]),
                                reader.GetString("ReviewText"),
                                reader.GetDateTime("ReviewDate")
                                ));
                        }
                    }
                }
                return ReviewDTo;
            }
        }
        public static List<ReviewDTO> GetAllReviewsByProductID(int ProductId)
        {
            List<ReviewDTO> ReviewDTo = new List<ReviewDTO>();
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllReviewsByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID",ProductId);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ReviewDTo.Add(new ReviewDTO(
                                reader.GetInt32("ReviewID"),
                                reader.GetInt32("ProductID"),
                                reader.GetInt32("CustomerID"),
                                Convert.ToSingle(reader["Rating"]),
                                reader.GetString("ReviewText"),
                                reader.GetDateTime("ReviewDate")
                                ));
                        }
                    }
                }
                return ReviewDTo;
            }
        }
        public static ReviewDTO GetReviewByID(int id)
        {
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetReviewByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ReviewID", id);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return(new ReviewDTO(
                                reader.GetInt32("ReviewID"),
                                reader.GetInt32("ProductID"),
                                reader.GetInt32("CustomerID"),
                                Convert.ToSingle(reader["Rating"]),
                                reader.GetString("ReviewText"),
                                reader.GetDateTime("ReviewDate")
                                ));
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
               
            }
        }
        public static int AddNewReview(ReviewDTO reviewDto)
        {
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_AddNewReview", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID",reviewDto.ProductID);
                    command.Parameters.AddWithValue("@CustomerID",reviewDto.CustomerID);
                    command.Parameters.AddWithValue("@Rating", reviewDto.Rating);
                    command.Parameters.AddWithValue("@ReviewText",reviewDto.ReviewText);
                    command.Parameters.AddWithValue("@ReviewDate", reviewDto.ReviewDate);
                    var outputIdParam = new SqlParameter("@ReviewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputIdParam);
                    connection.Open();
                    command.ExecuteNonQuery();
                    return (int)outputIdParam.Value;
                }
            }
        }
        public static bool UpdateReview(ReviewDTO reviewDto)
        {
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateReview", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ReviewID",reviewDto.ReviewID);
                    command.Parameters.AddWithValue("@ProductID", reviewDto.ProductID);
                    command.Parameters.AddWithValue("@CustomerID", reviewDto.CustomerID);
                    command.Parameters.AddWithValue("@Rating", reviewDto.Rating);
                    command.Parameters.AddWithValue("@ReviewText", reviewDto.ReviewText);
                    command.Parameters.AddWithValue("@ReviewDate", reviewDto.ReviewDate);
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }
        public static bool DeleteReview(int reviewId) {
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteReview", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ReviewID", reviewId);
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }
    }
}
