using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BAL
{
    public class LoginDTO
    {
        public string Username {  get; set; }
        public string Password { get; set; }

        public LoginDTO(string username,string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
    public class clsCustomer
    {
        public enum enMode { enAdd,enUpdate}
        public enMode Mode { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string emial { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public CustomerDTO CDTO
        {
            get
            {
                return new CustomerDTO(this.Id, this.Name, this.emial,this.phone,this.address,this.username,this.password);
            }
        }

        public clsCustomer(CustomerDTO dto,enMode mode = enMode.enAdd)
        {
            this.Id = dto.CustomerID;
            this.Name = dto.Name;
            this.emial = dto.Email;
            this.phone = dto.Phone;
            this.address = dto.Address;
            this.username = dto.UserName;
            this.password = dto.Password;
            this.Mode = mode;
        }
        public static List<CustomerDTO> GetAllCustomers()
        {
            return clsCustomerData.GetAllCustomers();
        }

        public static clsCustomer Find(int id)
        {
            CustomerDTO cdto = clsCustomerData.GetCustomerByID(id);
            if(cdto != null)
            {
                return new clsCustomer(cdto, enMode.enUpdate);
            }
            else
                return null;
        }

        private bool AddNewCustomer()
        {
            this.Id = clsCustomerData.AddNewCustomer(CDTO);
            return this.Id != -1;
        }

        private bool UpdateCustomer()
        {
            return clsCustomerData.UpdateCustomer(CDTO);
        }

        public bool Save()
        {
            switch (this.Mode)
            {
                case enMode.enAdd:
                    if (AddNewCustomer())
                    {
                        this.Mode = enMode.enUpdate;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case enMode.enUpdate:
                    return UpdateCustomer();
                    break;
                default:
                    return false;
            }
        }

        public static bool DeleteCustomer(int id) {
            return clsCustomerData.DeleteCustomer(id);
        }

        public static clsCustomer Login(LoginDTO loginDTO)
        {
            CustomerDTO cdto = clsCustomerData.LoginUser(loginDTO.Username,loginDTO.Password);
            if (cdto != null)
            {
                return new clsCustomer(cdto, enMode.enUpdate);
            }
            else
                return null;
        }
    }
}
