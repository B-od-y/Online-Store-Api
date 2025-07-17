using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class PaymentDTO
    {
        public int PaymentID { get; set; }
        public int OrderID { get; set; }
        public float Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime TransationDate { get; set; }
        public PaymentDTO(int paymentId, int orderId, float amount,string paymentMethod, DateTime transationDate)
        {
            PaymentID = paymentId;
            OrderID = orderId;
            Amount = amount;
            TransationDate = transationDate;
            PaymentMethod = paymentMethod;
        }
    }
    public class clsPaymentData
    {
        public static List<PaymentDTO> GetAllPayments()
        {
            List<PaymentDTO> PaymetDTo = new List<PaymentDTO>();
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllPayments", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PaymetDTo.Add(new PaymentDTO(
                                reader.GetInt32("PaymentID"),
                                reader.GetInt32("OrderID"),
                                Convert.ToSingle(reader["Amount"]),
                                reader.GetString("PaymentMethod"),
                                reader.GetDateTime("TransationDate")
                                ));
                        }
                    }
                }
                return PaymetDTo;
            }
        }
        public static PaymentDTO GetReviewByID(int id)
        {
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPaymentByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PaymentID", id);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (new PaymentDTO(
                                  reader.GetInt32("PaymentID"),
                                reader.GetInt32("OrderID"),
                                Convert.ToSingle(reader["Amount"]),
                                reader.GetString("PaymentMethod"),
                                reader.GetDateTime("TransationDate")
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
        public static int AddNewReview(PaymentDTO PaymetDTo)
        {
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_AddNewPayment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderID", PaymetDTo.OrderID);
                    command.Parameters.AddWithValue("@Amount", PaymetDTo.Amount);
                    command.Parameters.AddWithValue("@TransationDate", PaymetDTo.TransationDate);
                    command.Parameters.AddWithValue("@PaymentMethod", PaymetDTo.PaymentMethod);
                    var outputIdParam = new SqlParameter("@PaymentID", SqlDbType.Int)
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
        public static bool UpdateReview(PaymentDTO PaymetDTo)
        {
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdatePayment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PaymentID", PaymetDTo.PaymentID);
                    command.Parameters.AddWithValue("@OrderID", PaymetDTo.OrderID);
                    command.Parameters.AddWithValue("@Amount", PaymetDTo.Amount);
                    command.Parameters.AddWithValue("@TransationDate", PaymetDTo.TransationDate);
                    command.Parameters.AddWithValue("@PaymentMethod", PaymetDTo.PaymentMethod);
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }
        public static bool DeleteReview(int Id)
        {
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_DeletePayment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PaymentID", Id);
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }
    }
}
