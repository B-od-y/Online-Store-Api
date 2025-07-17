using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BAL
{
    public class clsReview
    {
        public enum enMode { enAdd = 0,enUpdate = 1}
        public enMode Mode { get; set; }
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public float Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewData { get; set; }
        public ReviewDTO RDTO
        {
            get
            {
                return new ReviewDTO(

                     this.ReviewId,
                    this.ProductId,
                     this.CustomerId,
                     this.Rating,
                    this.ReviewText,
                    this.ReviewData
                );
            }
        }
        public clsReview(ReviewDTO dto,enMode mode = enMode.enAdd)
        {
            this.ReviewId = dto.ReviewID;
            this.ProductId = dto.ProductID;
            this.CustomerId = dto.CustomerID;
            this.Rating = dto.Rating;
            this.ReviewText = dto.ReviewText;
            this.ReviewData = dto.ReviewDate;
            this.Mode = mode;
        }

        public static List<ReviewDTO> GetAllProductsReview()
        {
            return clsReviewData.GetAllReviews();
        }

        public static List<ReviewDTO> GetAllProductsReviewByProductId(int productId)
        {
            return clsReviewData.GetAllReviewsByProductID(productId);
        }

        public static clsReview Find(int id)
        {
            ReviewDTO dto = clsReviewData.GetReviewByID(id);
            if (dto == null)
            {
                return null;
            }
            return new clsReview(dto, enMode.enUpdate);
        }

        private bool AddNewReview()
        {
            this.ReviewId = clsReviewData.AddNewReview(this.RDTO);
            return (ReviewId != -1);
        }
        private bool UpdateReview()
        {
            return clsReviewData.UpdateReview(this.RDTO);
        }
        public bool Save()
        {
            switch (this.Mode)
            {
                case enMode.enAdd:
                    if (AddNewReview())
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
                    return UpdateReview();
                    break;
                default:
                    return false;
            }
        }

        public static bool Deleteview(int id)
        {
            return clsReviewData.DeleteReview(id);
        }
    }
}
