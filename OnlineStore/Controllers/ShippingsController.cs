using BAL;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        [HttpGet("All")]
        public ActionResult<IEnumerable<ShippingDTO>> GetAll()
        {
            var list = clsShipping.GetAll();
            return list.Count == 0 ? NotFound("No shippings found.") : Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<ShippingDTO> GetById(int id)
        {
            if (id < 1) return BadRequest("Invalid ID.");
            var shipping = clsShipping.Find(id);
            return shipping == null ? NotFound("Not found.") : Ok(shipping.DTO);
        }

        [HttpPost("Add")]
        public ActionResult Add([FromBody] ShippingDTO dto)
        {
            if (dto == null || dto.OrderID <= 0 || string.IsNullOrWhiteSpace(dto.CarrierName))
                return BadRequest("Invalid data.");

            var shipping = new clsShipping(dto);
            if (!shipping.Save()) return BadRequest("Insert failed.");

            dto.ShippingID = shipping.ShippingID;
            return CreatedAtAction(nameof(GetById), new { id = dto.ShippingID }, dto);
        }

        [HttpPut("Update/{id}")]
        public ActionResult Update(int id, [FromBody] ShippingDTO dto)
        {
            if (dto == null || dto.ShippingID != id)
                return BadRequest("Invalid data.");

            var existing = clsShipping.Find(id);
            if (existing == null) return NotFound("Shipping not found.");

            var updated = new clsShipping(dto, clsShipping.enMode.enUpdate);
            return updated.Save() ? Ok(updated.DTO) : BadRequest("Update failed.");
        }

        [HttpDelete("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 1) return BadRequest("Invalid ID.");
            return clsShipping.Delete(id) ? Ok("Deleted.") : NotFound("Shipping not found.");
        }
    }
}
