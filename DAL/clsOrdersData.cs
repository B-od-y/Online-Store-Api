using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public float TotalAmount { get; set; }
        public int StatusID { get; set; }

        public OrderDTO(int orderID, int customerID, DateTime orderDate, float totalAmount, int statusID)
        {
            OrderID = orderID;
            CustomerID = customerID;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
            StatusID = statusID;
        }
    }

    public static class clsOrderData
    {
        public static List<OrderDTO> GetAllOrders()
        {
            List<OrderDTO> list = new();

            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_GetAllOrders", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new OrderDTO(
                    reader.GetInt32("OrderID"),
                    reader.GetInt32("CustomerID"),
                    reader.GetDateTime("OrderDate"),
                    Convert.ToSingle(reader["TotalAmount"]),
                    reader.GetInt32("StatusID")
                ));
            }

            return list;
        }

        public static OrderDTO GetOrderByID(int id)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_GetOrderByID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderID", id);
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new OrderDTO(
                    reader.GetInt32("OrderID"),
                    reader.GetInt32("CustomerID"),
                    reader.GetDateTime("OrderDate"),
                    Convert.ToSingle(reader["TotalAmount"]),
                    reader.GetInt32("StatusID")
                );
            }

            return null;
        }

        public static int AddOrder(OrderDTO dto)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_AddNewOrder", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CustomerID", dto.CustomerID);
            cmd.Parameters.AddWithValue("@OrderDate", dto.OrderDate);
            cmd.Parameters.AddWithValue("@TotalAmount", dto.TotalAmount);
            cmd.Parameters.AddWithValue("@StatusID", dto.StatusID);

            var outParam = new SqlParameter("@OrderID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outParam);

            conn.Open();
            cmd.ExecuteNonQuery();

            return (int)outParam.Value;
        }

        public static bool UpdateOrder(OrderDTO dto)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_UpdateOrder", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@OrderID", dto.OrderID);
            cmd.Parameters.AddWithValue("@CustomerID", dto.CustomerID);
            cmd.Parameters.AddWithValue("@OrderDate", dto.OrderDate);
            cmd.Parameters.AddWithValue("@TotalAmount", dto.TotalAmount);
            cmd.Parameters.AddWithValue("@StatusID", dto.StatusID);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool DeleteOrder(int id)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_DeleteOrder", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderID", id);
            conn.Open();

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
