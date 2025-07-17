using DAL;
using System;
using System.Collections.Generic;

namespace BAL
{
    public class clsShipping
    {
        public enum enMode { enAdd, enUpdate }
        public enMode Mode { get; set; }

        public int ShippingID { get; set; }
        public int OrderID { get; set; }
        public string CarrierName { get; set; }
        public int ShippingStatusID { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }

        public ShippingDTO DTO => new(ShippingID, OrderID, CarrierName, ShippingStatusID, EstimatedDeliveryDate, ActualDeliveryDate);

        public clsShipping(ShippingDTO dto, enMode mode = enMode.enAdd)
        {
            ShippingID = dto.ShippingID;
            OrderID = dto.OrderID;
            CarrierName = dto.CarrierName;
            ShippingStatusID = dto.ShippingStatusID;
            EstimatedDeliveryDate = dto.EstimatedDeliveryDate;
            ActualDeliveryDate = dto.ActualDeliveryDate;
            Mode = mode;
        }

        public static List<ShippingDTO> GetAll() => clsShippingData.GetAllShippings();

        public static clsShipping Find(int id)
        {
            var dto = clsShippingData.GetShippingByID(id);
            return dto == null ? null : new clsShipping(dto, enMode.enUpdate);
        }

        private bool Add() => (ShippingID = clsShippingData.AddShipping(DTO)) != -1;

        private bool Update() => clsShippingData.UpdateShipping(DTO);

        public bool Save()
        {
            return Mode switch
            {
                enMode.enAdd => Add() && (Mode = enMode.enUpdate) == enMode.enUpdate,
                enMode.enUpdate => Update(),
                _ => false
            };
        }

        public static bool Delete(int id) => clsShippingData.DeleteShipping(id);
    }
}
