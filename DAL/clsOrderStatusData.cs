using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class OrderStatusDTO
    {
        public int OrderStatusID { get; set; }
        public string Status { get; set; }

        public OrderStatusDTO(int orderStatusID, string status)
        {
            OrderStatusID = OrderStatusID;
            Status = status;
        }
    }

    public static class clsOrderStatusData
    {
        public static List<OrderStatusDTO> GetAll()
        {
            List<OrderStatusDTO> list = new();

            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_GetAllOrderStatus", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new OrderStatusDTO(
                    reader.GetInt32("OrderStatusID"),
                    reader.GetString("Status")
                ));
            }

            return list;
        }

        public static OrderStatusDTO GetByID(int id)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_GetOrderStatusByID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderStatusID", id);
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new OrderStatusDTO(
                    reader.GetInt32("OrderStatusID"),
                    reader.GetString("Status")
                );
            }

            return null;
        }

        public static int Add(OrderStatusDTO dto)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_AddNewOrderStatus", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Status", dto.Status);
            var outParam = new SqlParameter("@OrderStatusID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outParam);

            conn.Open();
            cmd.ExecuteNonQuery();

            return (int)outParam.Value;
        }

        public static bool Update(OrderStatusDTO dto)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_UpdateOrderStatus", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@OrderStatusID", dto.OrderStatusID);
            cmd.Parameters.AddWithValue("@Status", dto.Status);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool Delete(int id)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_DeleteOrderStatus", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderStatusID", id);
            conn.Open();

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
