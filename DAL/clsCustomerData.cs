using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class CustomerDTO
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public CustomerDTO(int customerId,string name,string email,string phone,string address,string username,string password)
        {
            this.CustomerID = customerId;
            this.Name = name;
            this.Email = email;
            this.Phone = phone;
            this.Address = address;
            this.UserName = username;
            this.Password = password;
        }
    }

    public class clsCustomerData
    {
        public static List<CustomerDTO> GetAllCustomers()
        {
            List<CustomerDTO> CustomerDTo = new List<CustomerDTO>();
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllCustomers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustomerDTo.Add(new CustomerDTO(
                                reader.GetInt32("CustomerID"),
                                reader.GetString("Name"),
                                reader.GetString("Email"),
                                reader.GetString("Phone"),
                                reader.GetString("Address"),
                                reader.GetString("UserName"),
                                reader.GetString("PassWord")
                                ));
                        }
                    }
                }
                return CustomerDTo;
            }
        }
        public static CustomerDTO GetCustomerByID(int id)
        {
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetCustomerByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", id);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (new CustomerDTO(
                                  reader.GetInt32("CustomerID"),
                                reader.GetString("Name"),
                                reader.GetString("Email"),
                                reader.GetString("Phone"),
                                reader.GetString("Address"),
                                reader.GetString("UserName"),
                                reader.GetString("Password")
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
        public static CustomerDTO LoginUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_LoginUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", username);
                    command.Parameters.AddWithValue("@Password", password);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (new CustomerDTO(
                                  reader.GetInt32("CustomerID"),
                                reader.GetString("Name"),
                                reader.GetString("Email"),
                                reader.GetString("Phone"),
                                reader.GetString("Address"),
                                reader.GetString("UserName"),
                                reader.GetString("Password")
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
        public static int AddNewCustomer(CustomerDTO customerDto)
        {
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_AddNewCustomer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", customerDto.Name);
                    command.Parameters.AddWithValue("@Email", customerDto.Email);
                    command.Parameters.AddWithValue("@Phone", customerDto.Phone);
                    command.Parameters.AddWithValue("@Address", customerDto.Address);
                    command.Parameters.AddWithValue("@UserName", customerDto.UserName);
                    command.Parameters.AddWithValue("@Password", customerDto.Password);
                    var outputIdParam = new SqlParameter("@CustomerID", SqlDbType.Int)
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
        public static bool UpdateCustomer(CustomerDTO customerDto)
        {
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateCustomer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", customerDto.CustomerID);
                    command.Parameters.AddWithValue("@Name", customerDto.Name);
                    command.Parameters.AddWithValue("@Email", customerDto.Email);
                    command.Parameters.AddWithValue("@Phone", customerDto.Phone);
                    command.Parameters.AddWithValue("@Address", customerDto.Address);
                    command.Parameters.AddWithValue("@UserName", customerDto.UserName);
                    command.Parameters.AddWithValue("@Password", customerDto.Password);
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }
        public static bool DeleteCustomer(int customerId)
        {
            using (SqlConnection connection = new SqlConnection(clsSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteCustomer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", customerId);
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

    }

}

