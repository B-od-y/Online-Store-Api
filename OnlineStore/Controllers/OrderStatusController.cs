using Microsoft.AspNetCore.Mvc;
using DAL;
using BAL;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        [HttpGet("All")]
        public ActionResult<IEnumerable<OrderStatusDTO>> GetAll()
        {
            var list = clsOrderStatus.GetAll();
            return list.Count == 0 ? NotFound("No statuses found.") : Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<OrderStatusDTO> GetById(int id)
        {
            if (id < 1) return BadRequest("Invalid ID.");
            var status = clsOrderStatus.Find(id);
            return status == null ? NotFound("Not found.") : Ok(status.DTO);
        }

        [HttpPost("Add")]
        public ActionResult Add([FromBody] OrderStatusDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Status))
                return BadRequest("Invalid data.");

            var status = new clsOrderStatus(dto);
            if (!status.Save()) return BadRequest("Insert failed.");

            dto.OrderStatusID = status.OrderStatusID;
            return CreatedAtAction(nameof(GetById), new { id = dto.OrderStatusID }, dto);
        }

        [HttpPut("Update/{id}")]
        public ActionResult Update(int id, [FromBody] OrderStatusDTO dto)
        {
            if (dto == null || dto.OrderStatusID != id)
                return BadRequest("Invalid data.");

            var existing = clsOrderStatus.Find(id);
            if (existing == null) return NotFound("Not found.");

            var updated = new clsOrderStatus(dto, clsOrderStatus.enMode.enUpdate);
            return updated.Save() ? Ok(updated.DTO) : BadRequest("Update failed.");
        }

        [HttpDelete("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 1) return BadRequest("Invalid ID.");
            return clsOrderStatus.Delete(id) ? Ok("Deleted.") : NotFound("Not found.");
        }
    }
}
