using BAL;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllProductReviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<ReviewDTO>> GetAllProductReviews()
        {
            List<ReviewDTO> reviews = clsReview.GetAllProductsReview();

            if (reviews.Count == 0)
            {
                return NotFound("No product reviews found.");
            }

            return Ok(reviews);
        }

        [HttpGet("{id}", Name = "GetReviewById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ReviewDTO> GetReviewById(int id)
        {
            if (id < 1)
                return BadRequest($"Invalid review ID: {id}");

            clsReview review = clsReview.Find(id);

            if (review == null)
                return NotFound($"Review with ID {id} not found.");

            return Ok(review.RDTO);
        }

        [HttpGet("Product/{productId}", Name = "GetReviewsByProductId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<ReviewDTO>> GetReviewsByProductId(int productId)
        {
            List<ReviewDTO> reviews = clsReview.GetAllProductsReviewByProductId(productId);

            if (reviews.Count == 0)
            {
                return NotFound($"No reviews found for product ID: {productId}");
            }

            return Ok(reviews);
        }

        [HttpPost("Add", Name = "AddProductReview")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ReviewDTO> AddProductReview([FromBody] ReviewDTO newReviewDTO)
        {
            if (newReviewDTO == null || newReviewDTO.Rating < 0 || newReviewDTO.Rating > 5)
                return BadRequest("Invalid review data.");

            clsReview review = new clsReview(newReviewDTO, clsReview.enMode.enAdd);
            if (review.Save())
            {
                newReviewDTO.ReviewID = review.ReviewId;
                return CreatedAtRoute("GetReviewById", new { id = newReviewDTO.ReviewID }, newReviewDTO);
            }

            return BadRequest("Failed to add review.");
        }

        [HttpPut("Update/{id}", Name = "UpdateProductReview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ReviewDTO> UpdateProductReview(int id, [FromBody] ReviewDTO updatedReviewDTO)
        {
            if (updatedReviewDTO == null || updatedReviewDTO.ReviewID != id)
                return BadRequest("Invalid review data or ID mismatch.");

            clsReview existingReview = clsReview.Find(id);
            if (existingReview == null)
                return NotFound($"Review with ID {id} not found.");

            clsReview reviewToUpdate = new clsReview(updatedReviewDTO, clsReview.enMode.enUpdate);

            if (reviewToUpdate.Save())
                return Ok(reviewToUpdate.RDTO);

            return BadRequest("Failed to update review.");
        }

        [HttpDelete("Delete/{id}", Name = "DeleteProductReview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteProductReview(int id)
        {
            if (id < 1)
                return BadRequest($"Invalid ID: {id}");

            if (clsReview.Deleteview(id))
                return Ok($"Review with ID {id} has been deleted.");

            return NotFound($"Review with ID {id} not found.");
        }
    }
}
