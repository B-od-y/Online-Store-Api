using BAL;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllProductsCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CategoryDTO>> GetAllProducts()
        {
            List<CategoryDTO> CategorysList = clsCategory.GetAllProductsCategory();
            if (CategorysList.Count == 0)
            {
                return NotFound("No ProductsCategory Found!");
            }
            return Ok(CategorysList);
        }


        [HttpGet("{id}", Name = "GetCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CategoryDTO> GetCategoryById(int id)
        {

            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            //var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            //if (student == null)
            //{
            //    return NotFound($"Student with ID {id} not found.");
            //}
            clsCategory category = clsCategory.Find(id);

            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            //here we get only the DTO object to send it back.
            CategoryDTO SDTO = category.CgDTO;

            //we return the DTO not the student object.
            return Ok(SDTO);

        }


        [HttpPost("AddNew", Name = "AddNewProductCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CategoryDTO> AddNewProductCategory(CategoryDTO newCategoryDTO)
        {
            //we validate the data here
            if (newCategoryDTO == null || string.IsNullOrEmpty(newCategoryDTO.CategoryName))
            {
                return BadRequest("Invalid Category data.");
            }

            //newStudent.Id = StudentDataSimulation.StudentsList.Count > 0 ? StudentDataSimulation.StudentsList.Max(s => s.Id) + 1 : 1;

            clsCategory category = new clsCategory(new CategoryDTO(newCategoryDTO.CategoryID, newCategoryDTO.CategoryName));
            category.save();

            newCategoryDTO.CategoryID = category.CategoryID;


            return CreatedAtRoute("GetCategoryById", new { id = newCategoryDTO.CategoryID }, newCategoryDTO);
        }


        [HttpPut("Update{id}", Name = "UpdateProductCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CategoryDTO> UpdateProductCategory(int id, CategoryDTO UpdatedCategoryDTO)
        {
            if (UpdatedCategoryDTO == null || string.IsNullOrEmpty(UpdatedCategoryDTO.CategoryName))
            {
                return BadRequest("Invalid Category data.");
            }

            clsCategory clscategory = clsCategory.Find(id);

            if (clscategory == null) {
                return NotFound($"Category with ID {id} not found.");
            }

            clscategory.CategoryName = UpdatedCategoryDTO.CategoryName;
            clscategory.save();

            return Ok(clscategory.CgDTO);
        }


        [HttpDelete("Delete{id}", Name = "DeleteProductCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteProductCategory(int id) {

            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            if (clsCategory.DeleteProductCategory(id))
            {
                return Ok($"Student with ID {id} has been deleted.");
            }
            else
                return NotFound($"Student with ID {id} not found. no rows deleted!");
        }
    }
} 
