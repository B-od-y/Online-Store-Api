using DAL;
using System;
using System.Collections.Generic;

namespace BAL
{
    public class clsPayment
    {
        public enum enMode { enAdd, enUpdate }
        public enMode Mode { get; set; }

        public int PaymentID { get; set; }
        public int OrderID { get; set; }
        public float Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime TransationDate { get; set; }

        public PaymentDTO DTO
        {
            get
            {
                return new PaymentDTO(PaymentID, OrderID, Amount, PaymentMethod, TransationDate);
            }
        }

        public clsPayment(PaymentDTO dto, enMode mode = enMode.enAdd)
        {
            this.PaymentID = dto.PaymentID;
            this.OrderID = dto.OrderID;
            this.Amount = dto.Amount;
            this.PaymentMethod = dto.PaymentMethod;
            this.TransationDate = dto.TransationDate;
            this.Mode = mode;
        }

        public static List<PaymentDTO> GetAllPayments()
        {
            return clsPaymentData.GetAllPayments();
        }

        public static clsPayment Find(int id)
        {
            var dto = clsPaymentData.GetReviewByID(id);
            if (dto != null)
                return new clsPayment(dto, enMode.enUpdate);
            else
                return null;
        }

        private bool AddNewPayment()
        {
            this.PaymentID = clsPaymentData.AddNewReview(this.DTO);
            return this.PaymentID != -1;
        }

        private bool UpdatePayment()
        {
            return clsPaymentData.UpdateReview(this.DTO);
        }

        public bool Save()
        {
            switch (this.Mode)
            {
                case enMode.enAdd:
                    if (AddNewPayment())
                    {
                        this.Mode = enMode.enUpdate;
                        return true;
                    }
                    else
                        return false;

                case enMode.enUpdate:
                    return UpdatePayment();

                default:
                    return false;
            }
        }

        public static bool Delete(int id)
        {
            return clsPaymentData.DeleteReview(id);
        }
    }
}
