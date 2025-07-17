using BAL;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllPayments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<PaymentDTO>> GetAllPayments()
        {
            var payments = clsPayment.GetAllPayments();

            if (payments.Count == 0)
                return NotFound("No payments found.");

            return Ok(payments);
        }

        [HttpGet("{id}", Name = "GetPaymentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PaymentDTO> GetPaymentById(int id)
        {
            if (id < 1)
                return BadRequest($"Invalid payment ID: {id}");

            var payment = clsPayment.Find(id);

            if (payment == null)
                return NotFound($"Payment with ID {id} not found.");

            return Ok(payment.DTO);
        }

        [HttpPost("Add", Name = "AddPayment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PaymentDTO> AddPayment([FromBody] PaymentDTO newPaymentDTO)
        {
            if (newPaymentDTO == null || newPaymentDTO.OrderID < 1 || newPaymentDTO.Amount <= 0)
                return BadRequest("Invalid payment data.");

            var payment = new clsPayment(newPaymentDTO, clsPayment.enMode.enAdd);

            if (payment.Save())
            {
                newPaymentDTO.PaymentID = payment.PaymentID;
                return CreatedAtRoute("GetPaymentById", new { id = newPaymentDTO.PaymentID }, newPaymentDTO);
            }

            return BadRequest("Failed to add payment.");
        }

        [HttpPut("Update/{id}", Name = "UpdatePayment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PaymentDTO> UpdatePayment(int id, [FromBody] PaymentDTO updatedDTO)
        {
            if (updatedDTO == null || updatedDTO.PaymentID != id)
                return BadRequest("Invalid payment data or ID mismatch.");

            var existingPayment = clsPayment.Find(id);
            if (existingPayment == null)
                return NotFound($"Payment with ID {id} not found.");

            var paymentToUpdate = new clsPayment(updatedDTO, clsPayment.enMode.enUpdate);

            if (paymentToUpdate.Save())
                return Ok(paymentToUpdate.DTO);

            return BadRequest("Failed to update payment.");
        }

        [HttpDelete("Delete/{id}", Name = "DeletePayment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeletePayment(int id)
        {
            if (id < 1)
                return BadRequest("Invalid ID.");

            if (clsPayment.Delete(id))
                return Ok($"Payment with ID {id} deleted.");

            return NotFound($"Payment with ID {id} not found.");
        }
    }
}
