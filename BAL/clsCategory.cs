using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Microsoft.Identity.Client;

namespace BAL
{
    public class clsCategory
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        
        public CategoryDTO CgDTO
        {
            get { return new CategoryDTO(this.CategoryID, this.CategoryName); }
        }//علشان ارجع لل داتا الكلاس بتاعيه بالبيانات اللي معايا

        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public clsCategory(CategoryDTO CDTO,enMode mode = enMode.AddNew)
        {
            this.CategoryID = CDTO.CategoryID;
            this.CategoryName = CDTO.CategoryName;
            this.Mode = mode;
        }

        public static List<CategoryDTO> GetAllProductsCategory()
        {
            return clsCategoryData.GetAllProductsCategory();
        }

        private bool AddNewCategory()
        {
            this.CategoryID = clsCategoryData.AddNewCategory(CgDTO);
            return (this.CategoryID != -1);
        }

        private bool UpdateCategory()
        {
            if (clsCategoryData.UpdateCategory(CgDTO))
            {
                return true;
            }
            return false;
        }

        public static bool DeleteProductCategory(int CategoryID)
        {
            return clsCategoryData.DeleteCategory(CategoryID);
        }

        public static clsCategory Find(int ID)
        {
            CategoryDTO CDTO =  clsCategoryData.GetCategoryByID(ID);
            if (CDTO != null)
            {
                return new clsCategory(CDTO, enMode.Update);
            }
            else
            {
                return null;
            }
        }
        public bool save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (AddNewCategory())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                 case enMode.Update:
                     return UpdateCategory();
                default:
                    return false;
          
        }   }
    }
}
