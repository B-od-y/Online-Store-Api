using Microsoft.AspNetCore.Mvc;
using DAL;
using BAL;
namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet("All")]
        public ActionResult<IEnumerable<OrderDTO>> GetAll()
        {
            var list = clsOrder.GetAll();
            return list.Count == 0 ? NotFound("No orders found.") : Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<OrderDTO> GetById(int id)
        {
            if (id < 1) return BadRequest("Invalid ID.");
            var order = clsOrder.Find(id);
            return order == null ? NotFound("Order not found.") : Ok(order.DTO);
        }

        [HttpPost("Add")]
        public ActionResult Add([FromBody] OrderDTO dto)
        {
            if (dto == null || dto.CustomerID < 1 || dto.TotalAmount <= 0)
                return BadRequest("Invalid data.");

            var order = new clsOrder(dto);
            if (!order.Save()) return BadRequest("Insert failed.");

            dto.OrderID = order.OrderID;
            return CreatedAtAction(nameof(GetById), new { id = dto.OrderID }, dto);
        }

        [HttpPut("Update/{id}")]
        public ActionResult Update(int id, [FromBody] OrderDTO dto)
        {
            if (dto == null || dto.OrderID != id)
                return BadRequest("Invalid data.");

            var existing = clsOrder.Find(id);
            if (existing == null) return NotFound("Order not found.");

            var updated = new clsOrder(dto, clsOrder.enMode.enUpdate);
            return updated.Save() ? Ok(updated.DTO) : BadRequest("Update failed.");
        }

        [HttpDelete("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 1) return BadRequest("Invalid ID.");
            return clsOrder.Delete(id) ? Ok("Deleted.") : NotFound("Order not found.");
        }
    }
}
