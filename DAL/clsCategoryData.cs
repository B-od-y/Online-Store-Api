using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;

namespace DAL
{
    public class CategoryDTO
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public CategoryDTO(int categoryID, string categoryName)
        {
            CategoryID = categoryID;
            CategoryName = categoryName;
        }
    }
    public class clsCategoryData
    {
        public static List<CategoryDTO> GetAllProductsCategory()
        {
            List<CategoryDTO> productsCategory = new List<CategoryDTO>();
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllProductsCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productsCategory.Add(new CategoryDTO(
                                reader.GetInt32("CategoryID"),
                                reader.GetString("CategoryName")));
                        }
                    }
                }
                return productsCategory;
            }
        }
        public static CategoryDTO GetCategoryByID(int ID)
        {
            
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetProductCategoryByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CategoryID", ID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {

                            return new CategoryDTO(reader.GetInt32("CategoryID"), reader.GetString("CategoryName"));
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
               
            }
        }
      
        public static int AddNewCategory(CategoryDTO categoryDTO)
        {
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_AddNewProductCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CategoryName", categoryDTO.CategoryName);
                    var outputIdParam = new SqlParameter("@CategoryID", SqlDbType.Int)
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

        public static bool UpdateCategory(CategoryDTO categoryDTO) {
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_AddNewProductCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CategoryID", categoryDTO.CategoryID);
                    command.Parameters.AddWithValue("@CategoryName", categoryDTO.CategoryName);
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }

        }

        public static bool DeleteCategory(int CategoryID)
        {
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteProductCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CategoryID", CategoryID);
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }

        }
    }
}



