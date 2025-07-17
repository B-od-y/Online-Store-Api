using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class ShippingDTO
    {
        public int ShippingID { get; set; }
        public int OrderID { get; set; }
        public string CarrierName { get; set; }
        public int ShippingStatusID { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }

        public ShippingDTO(int shippingID, int orderID, string carrierName, int shippingStatusID, DateTime estimatedDeliveryDate, DateTime? actualDeliveryDate)
        {
            ShippingID = shippingID;
            OrderID = orderID;
            CarrierName = carrierName;
            ShippingStatusID = shippingStatusID;
            EstimatedDeliveryDate = estimatedDeliveryDate;
            ActualDeliveryDate = actualDeliveryDate;
        }
    }

    public static class clsShippingData
    {
        public static List<ShippingDTO> GetAllShippings()
        {
            List<ShippingDTO> list = new();
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_GetAllShippings", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new ShippingDTO(
                    reader.GetInt32("ShippingID"),
                    reader.GetInt32("OrderID"),
                    reader.GetString("CarrierName"),
                    reader.GetInt32("ShippingStatusID"),
                    reader.GetDateTime("EstimatedDeliveryDate"),
                    reader.IsDBNull("ActualDeliveryDate") ? null : reader.GetDateTime("ActualDeliveryDate")
                ));
            }

            return list;
        }

        public static ShippingDTO GetShippingByID(int id)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_GetShippingByID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ShippingID", id);
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new ShippingDTO(
                    reader.GetInt32("ShippingID"),
                    reader.GetInt32("OrderID"),
                    reader.GetString("CarrierName"),
                    reader.GetInt32("ShippingStatusID"),
                    reader.GetDateTime("EstimatedDeliveryDate"),
                    reader.IsDBNull("ActualDeliveryDate") ? null : reader.GetDateTime("ActualDeliveryDate")
                );
            }

            return null;
        }

        public static int AddShipping(ShippingDTO dto)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_AddShipping", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@OrderID", dto.OrderID);
            cmd.Parameters.AddWithValue("@CarrierName", dto.CarrierName);
            cmd.Parameters.AddWithValue("@ShippingStatusID", dto.ShippingStatusID);
            cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", dto.EstimatedDeliveryDate);
            cmd.Parameters.AddWithValue("@ActualDeliveryDate", (object?)dto.ActualDeliveryDate ?? DBNull.Value);

            var outParam = new SqlParameter("@ShippingID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outParam);

            conn.Open();
            cmd.ExecuteNonQuery();

            return (int)outParam.Value;
        }

        public static bool UpdateShipping(ShippingDTO dto)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_UpdateShipping", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ShippingID", dto.ShippingID);
            cmd.Parameters.AddWithValue("@OrderID", dto.OrderID);
            cmd.Parameters.AddWithValue("@CarrierName", dto.CarrierName);
            cmd.Parameters.AddWithValue("@ShippingStatusID", dto.ShippingStatusID);
            cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", dto.EstimatedDeliveryDate);
            cmd.Parameters.AddWithValue("@ActualDeliveryDate", (object?)dto.ActualDeliveryDate ?? DBNull.Value);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool DeleteShipping(int id)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_DeleteShipping", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ShippingID", id);
            conn.Open();

            return cmd.ExecuteNonQuery() > 0;
        }
    }

}
