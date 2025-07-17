using BAL;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllCustomers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CustomerDTO>> GetAllCustomers()
        {
            var customers = clsCustomer.GetAllCustomers();

            if (customers.Count == 0)
                return NotFound("No customers found.");

            return Ok(customers);
        }

        [HttpGet("{id}", Name = "GetCustomerById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CustomerDTO> GetCustomerById(int id)
        {
            if (id < 1)
                return BadRequest($"Invalid ID: {id}");

            var customer = clsCustomer.Find(id);

            if (customer == null)
                return NotFound($"Customer with ID {id} not found.");

            return Ok(customer.CDTO);
        }
        [HttpPost("Login", Name = "LoginUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CustomerDTO> GetCustomerById(LoginDTO loginDTO)
        {
            if (loginDTO.Username.Length == 0 ||loginDTO.Password.Length == 0)
                return BadRequest($"Invalid Input");

            var customer = clsCustomer.Login(loginDTO);

            if (customer == null)
                return NotFound($"invalid password or username");

            return Ok(customer.CDTO);
        }





        [HttpPost("Add", Name = "AddCustomer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CustomerDTO> AddCustomer([FromBody] CustomerDTO newCustomerDTO)
        {
            if (newCustomerDTO == null || string.IsNullOrWhiteSpace(newCustomerDTO.UserName))
                return BadRequest("Invalid customer data.");

            var customer = new clsCustomer(newCustomerDTO, clsCustomer.enMode.enAdd);

            if (customer.Save())
            {
                newCustomerDTO.CustomerID = customer.Id;
                return CreatedAtRoute("GetCustomerById", new { id = newCustomerDTO.CustomerID }, newCustomerDTO);
            }

            return BadRequest("Failed to add customer.");
        }

        [HttpPut("Update/{id}", Name = "UpdateCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CustomerDTO> UpdateCustomer(int id, [FromBody] CustomerDTO updatedCustomerDTO)
        {
            if (updatedCustomerDTO == null || updatedCustomerDTO.CustomerID != id)
                return BadRequest("Invalid customer data or ID mismatch.");

            var existingCustomer = clsCustomer.Find(id);
            if (existingCustomer == null)
                return NotFound($"Customer with ID {id} not found.");

            var customerToUpdate = new clsCustomer(updatedCustomerDTO, clsCustomer.enMode.enUpdate);

            if (customerToUpdate.Save())
                return Ok(customerToUpdate.CDTO);

            return BadRequest("Failed to update customer.");
        }

        [HttpDelete("Delete/{id}", Name = "DeleteCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteCustomer(int id)
        {
            if (id < 1)
                return BadRequest($"Invalid ID: {id}");

            if (clsCustomer.DeleteCustomer(id))
                return Ok($"Customer with ID {id} deleted.");

            return NotFound($"Customer with ID {id} not found.");
        }
    }
}
