using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class OrderItemDTO
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }  // هيتحدد تلقائيًا

        public OrderItemDTO(int orderID, int productID, int quantity, float price)
        {
            OrderID = orderID;
            ProductID = productID;
            Quantity = quantity;
            Price = price;
        }
    }


    public static class clsOrderItemData
    {
        public static bool AddOrderItem(OrderItemDTO dto)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_AddNewOrderItem", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@OrderID", dto.OrderID);
            cmd.Parameters.AddWithValue("@ProductID", dto.ProductID);
            cmd.Parameters.AddWithValue("@Quantity", dto.Quantity);
            cmd.Parameters.AddWithValue("@Price", dto.Price);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public static List<OrderItemDTO> GetItemsByOrder(int orderID)
        {
            var list = new List<OrderItemDTO>();
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SELECT * FROM OrderItems WHERE OrderID = @OrderID", conn)
            {
                CommandType = CommandType.Text
            };

            cmd.Parameters.AddWithValue("@OrderID", orderID);
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new OrderItemDTO(
                    reader.GetInt32("OrderID"),
                    reader.GetInt32("ProductID"),
                    reader.GetInt32("Quantity"),
                    Convert.ToSingle(reader["Price"])
                ));
            }
            return list;
        }

        public static OrderItemDTO GetItemByOrderAndProduct(int orderID, int productID)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_GetOrderItem", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@OrderID", orderID);
            cmd.Parameters.AddWithValue("@ProductID", productID);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new OrderItemDTO(
                    reader.GetInt32("OrderID"),
                    reader.GetInt32("ProductID"),
                    reader.GetInt32("Quantity"),
                    Convert.ToSingle(reader["Price"])
                );
            }

            return null;
        }

        public static bool DeleteOrderItem(int orderID, int productID)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_DeleteOrderItem", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@OrderID", orderID);
            cmd.Parameters.AddWithValue("@ProductID", productID);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }


}
