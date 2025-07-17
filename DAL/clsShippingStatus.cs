using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class ShippingStatusDTO
    {
        public int ShippingStatusID { get; set; }
        public string Status { get; set; }

        public ShippingStatusDTO(int shippingStatusID, string status)
        {
            ShippingStatusID = shippingStatusID;
            Status = status;
        }

    }

    public static class clsShippingStatusData
    {
        public static List<ShippingStatusDTO> GetAllStatuses()
        {
            List<ShippingStatusDTO> list = new();

            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_GetAllShippingStatus", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new ShippingStatusDTO(
                    reader.GetInt32("ShippingStatusID"),
                    reader.GetString("Status")
                ));
            }

            return list;
        }

        public static ShippingStatusDTO GetStatusByID(int id)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_GetShippingStatusByID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ShippingStatusID", id);
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new ShippingStatusDTO(
                    reader.GetInt32("ShippingStatusID"),
                    reader.GetString("Status")
                );
            }

            return null;
        }

        public static int AddStatus(ShippingStatusDTO dto)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_AddNewShippingStatus", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Status", dto.Status);
            var outParam = new SqlParameter("@ShippingStatusID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outParam);

            conn.Open();
            cmd.ExecuteNonQuery();

            return (int)outParam.Value;
        }

        public static bool UpdateStatus(ShippingStatusDTO dto)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_UpdateShippingStatus", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ShippingStatusID", dto.ShippingStatusID);
            cmd.Parameters.AddWithValue("@Status", dto.Status);

            conn.Open();
            cmd.ExecuteNonQuery();
            return true;
            
        }

        public static bool DeleteStatus(int id)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_DeleteShippingStatus", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ShippingStatusID", id);
            conn.Open();

            cmd.ExecuteNonQuery();
            return true;
        }
    }
}
