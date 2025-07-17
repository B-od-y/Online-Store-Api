using DAL;
using System.Collections.Generic;

namespace BAL
{
    public class clsOrderItem
    {
        public enum enMode { enAdd, enUpdate }

        public enMode Mode { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }

        public OrderItemDTO DTO
        {
            get
            {
                return new OrderItemDTO(OrderID, ProductID, Quantity, Price);
            }
        }

        public clsOrderItem(OrderItemDTO dto, enMode mode = enMode.enAdd)
        {
            this.OrderID = dto.OrderID;
            this.ProductID = dto.ProductID;
            this.Quantity = dto.Quantity;
            this.Price = clsProductData.GetPriceByID(dto.ProductID);
            this.Mode = mode;
        }

        private bool Add()
        {
            return clsOrderItemData.AddOrderItem(this.DTO);
        }

        private bool Update()
        {
            
            return false; // Placeholder
        }

        public bool Save()
        {
            if (this.Mode == enMode.enAdd)
            {
                return Add();
            }
            else if (this.Mode == enMode.enUpdate)
            {
                return Update();
            }

            return false;
        }

        public static List<OrderItemDTO> GetItemsByOrder(int orderID)
        {
            return clsOrderItemData.GetItemsByOrder(orderID);
        }

        public static OrderItemDTO GetItemByOrderAndProduct(int orderID, int productID)
        {
            return clsOrderItemData.GetItemByOrderAndProduct(orderID, productID);
        }

        public static bool Delete(int orderID, int productID)
        {
            return clsOrderItemData.DeleteOrderItem(orderID, productID);
        }
    }
}
