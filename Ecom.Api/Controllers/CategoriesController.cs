using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interface;
using Ecom.Core.Models.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{
    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var categories = await unitOfWork.CategoryRepository.GetAllAsync();
                if (categories is null)
                {
                    return BadRequest(new ResponseAPI(400, "Not Data Found "));
                }
                return Ok(categories);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetbyId(int id)
        {
            try
            {
                var category = await unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (category is null)
                {
                    return BadRequest(new ResponseAPI(400, $"No Data Found for this id {id}"));
                }
                return Ok(category);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("add-Category")]
        public async Task<IActionResult> Add(AddCategoryDto categoryDto)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDto);

                await unitOfWork.CategoryRepository.AddAsync(category);

                return Ok(new ResponseAPI(200, "Item has been Added"));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("Update-Category")]
        public async Task<IActionResult> Update(UpdateCategoryDto categoryDto)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDto);
                await unitOfWork.CategoryRepository.UpdateAsync(category);
                return Ok(new ResponseAPI(200, "Item has been Updated"));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Delete-Category/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await unitOfWork.CategoryRepository.DeleteAsync(id);
                return Ok(new ResponseAPI(200, "Item has been Deelted"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
