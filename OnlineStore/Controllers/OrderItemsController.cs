using Microsoft.AspNetCore.Mvc;
using BAL;
using DAL;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        [HttpGet("ByOrder/{orderID}")]
        public ActionResult<List<OrderItemDTO>> GetItemsByOrder(int orderID)
        {
            var items = clsOrderItem.GetItemsByOrder(orderID);
            if (items == null || items.Count == 0)
                return NotFound($"No items found for OrderID = {orderID}");

            return Ok(items);
        }

        [HttpGet("Item")]
        public ActionResult<OrderItemDTO> GetItemByOrderAndProduct([FromQuery] int orderID, [FromQuery] int productID)
        {
            var item = clsOrderItem.GetItemByOrderAndProduct(orderID, productID);
            if (item == null)
                return NotFound($"No item found for OrderID = {orderID} and ProductID = {productID}");

            return Ok(item);
        }

        [HttpPost("Add")]
        public ActionResult AddItem(OrderItemDTO dto)
        {
            var item = new clsOrderItem(dto, clsOrderItem.enMode.enAdd);
            if (item.Save())
                return Ok("Order item added successfully.");
            else
                return BadRequest("Failed to add order item.");
        }

        [HttpDelete("Delete")]
        public ActionResult DeleteItem([FromQuery] int orderID, [FromQuery] int productID)
        {
            if (clsOrderItem.Delete(orderID, productID))
                return Ok("Order item deleted successfully.");
            else
                return NotFound("Order item not found.");
        }
    }
}
