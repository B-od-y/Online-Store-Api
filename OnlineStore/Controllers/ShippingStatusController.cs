using Microsoft.AspNetCore.Mvc;
using DAL;
using BAL;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingStatusController : ControllerBase
    {
        [HttpGet("All")]
        public ActionResult<IEnumerable<ShippingStatusDTO>> GetAll()
        {
            var list = clsShippingStatus.GetAll();
            return list.Count == 0 ? NotFound("No statuses found.") : Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<ShippingStatusDTO> GetById(int id)
        {
            if (id < 1) return BadRequest("Invalid ID.");
            var status = clsShippingStatus.Find(id);
            return status == null ? NotFound("Not found.") : Ok(status.DTO);
        }

        [HttpPost("Add")]
        public ActionResult Add([FromBody] ShippingStatusDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Status))
                return BadRequest("Invalid data.");

            var status = new clsShippingStatus(dto);
            if (!status.Save()) return BadRequest("Insert failed.");

            dto.ShippingStatusID = status.ShippingStatusID;
            return CreatedAtAction(nameof(GetById), new { id = dto.ShippingStatusID }, dto);
        }

        [HttpPut("Update/{id}")]
        public ActionResult Update(int id, [FromBody] ShippingStatusDTO dto)
        {
            if (dto == null)
                return BadRequest("Invalid data.");

            var existing = clsShippingStatus.Find(id);
            if (existing == null) return NotFound("Not found.");

            var updated = new clsShippingStatus(dto, clsShippingStatus.enMode.enUpdate);
            return updated.Save() ? Ok(updated.DTO) : BadRequest("Update failed.");
        }

        [HttpDelete("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 1) return BadRequest("Invalid ID.");
            return clsShippingStatus.Delete(id) ? Ok("Deleted.") : NotFound("Not found.");
        }
    }
}
