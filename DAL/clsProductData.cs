using System.Data;
using System.Runtime.InteropServices;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class ProductDTO
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int QuantityInStock { get; set; }
        public int CatagoryID { get; set; }
        public string ImageURL { get; set; }

        public ProductDTO(int productID, string productName, string description, float price, int quantityInStock, int catagoryID, string imageURL)
        {
            ProductID = productID;
            ProductName = productName;
            Description = description;
            Price = price;
            QuantityInStock = quantityInStock;
            CatagoryID = catagoryID;
            ImageURL = imageURL;
        }
    }

    public static class clsProductData
    {
        public static List<ProductDTO> GetAllProducts()
        {
            List<ProductDTO> list = new();

            using SqlConnection conn = new(clsSettings.Connection);
            
                using SqlCommand cmd = new("SP_GetAllProducts", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new ProductDTO(
                        reader.GetInt32("ProductID"),
                        reader.GetString("ProductName"),
                        reader.GetString("Description"),
                        Convert.ToSingle(reader["Price"]),
                        reader.GetInt32("QuantityinStock"),
                        reader.GetInt32("CatagoryID"),
                        reader.GetString("ImageURL")
                    ));
                }
            

            return list;
        } 

        public static ProductDTO GetProductByID(int id)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_GetProductByID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductID", id);
            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new ProductDTO(
                    reader.GetInt32("ProductID"),
                    reader.GetString("ProductName"),
                    reader.GetString("Description"),
                   Convert.ToSingle(reader["Price"]),
                    reader.GetInt32("QuantityinStock"),
                    reader.GetInt32("CatagoryID"),
                    reader.GetString("ImageURL")
                );
            }

            return null;
        }
        public static float GetPriceByID(int productId)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SELECT Price FROM ProductCatalog WHERE ProductID = @ProductID", conn);
            cmd.Parameters.AddWithValue("@ProductID", productId);
            conn.Open();

            object result = cmd.ExecuteScalar();
            return result != null ? Convert.ToSingle(result) : -1;
        }
        public static int AddProduct(ProductDTO dto)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_AddNewProduct", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ProductName", dto.ProductName);
            cmd.Parameters.AddWithValue("@Description", dto.Description);
            cmd.Parameters.AddWithValue("@Price", dto.Price);
            cmd.Parameters.AddWithValue("@QuantityInStock", dto.QuantityInStock);
            cmd.Parameters.AddWithValue("@CatagoryID", dto.CatagoryID);
            cmd.Parameters.AddWithValue("@ImageURL", dto.ImageURL);

            var outParam = new SqlParameter("@ProductID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outParam);

            conn.Open();
            cmd.ExecuteNonQuery();

            return (int)outParam.Value;
        }

        public static bool UpdateProduct(ProductDTO dto)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_UpdateProduct", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ProductID", dto.ProductID);
            cmd.Parameters.AddWithValue("@ProductName", dto.ProductName);
            cmd.Parameters.AddWithValue("@Description", dto.Description);
            cmd.Parameters.AddWithValue("@Price", dto.Price);
            cmd.Parameters.AddWithValue("@QuantityInStock", dto.QuantityInStock);
            cmd.Parameters.AddWithValue("@CatagoryID", dto.CatagoryID);
            cmd.Parameters.AddWithValue("@ImageURL", dto.ImageURL);

            conn.Open();
            cmd.ExecuteNonQuery();
            return true;
        }

        public static bool DeleteProduct(int id)
        {
            using SqlConnection conn = new(clsSettings.Connection);
            using SqlCommand cmd = new("SP_DeleteProduct", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductID", id);
            conn.Open();

            cmd.ExecuteNonQuery();
            return true;
            
        }
    }
}