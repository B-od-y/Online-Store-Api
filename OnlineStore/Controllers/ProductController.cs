using Microsoft.AspNetCore.Mvc;
using DAL;
using BAL;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet("All")]
        public ActionResult<IEnumerable<ProductDTO>> GetAll()
        {
            var list = clsProduct.GetAll();
            return list.Count == 0 ? NotFound("No products found.") : Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductDTO> GetById(int id)
        {
            if (id < 1) return BadRequest("Invalid ID.");
            var product = clsProduct.Find(id);
            return product == null ? NotFound("Product not found.") : Ok(product.DTO);
        }

        [HttpPost("Add")]
        public ActionResult Add([FromBody] ProductDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.ProductName))
                return BadRequest("Invalid data.");

            var product = new clsProduct(dto);
            if (!product.Save()) return BadRequest("Insert failed.");

            dto.ProductID = product.ProductID;
            return CreatedAtAction(nameof(GetById), new { id = dto.ProductID }, dto);
        }

        [HttpPut("Update/{id}")]
        public ActionResult Update(int id, [FromBody] ProductDTO dto)
        {
            if (dto == null || dto.ProductID != id)
                return BadRequest("Invalid data.");

            var existing = clsProduct.Find(id);
            if (existing == null) return NotFound("Product not found.");

            var updated = new clsProduct(dto, clsProduct.enMode.enUpdate);
            return updated.Save() ? Ok(updated.DTO) : BadRequest("Update failed.");
        }

        [HttpDelete("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 1) return BadRequest("Invalid ID.");
            return clsProduct.Delete(id) ? Ok("Deleted.") : NotFound("Product not found.");
        }
    }
}
