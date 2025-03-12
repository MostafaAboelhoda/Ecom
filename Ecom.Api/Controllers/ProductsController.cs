using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interface;
using Ecom.Core.Models.Product;
using Ecom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> Get([FromQuery] ProductParams productParams)
        {
            try
            {
                var products = await unitOfWork.ProductRepository
                                .GetAllAsync(productParams);

                if (products is null)
                {
                    return BadRequest(new ResponseAPI(400));
                }
                return Ok(new Pagination<ProductDto>(productParams.PageNumber,
                    productParams.PageSize, products.TotalCount, products.Products));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpGet("get-by-Id/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var product = await unitOfWork.ProductRepository
                                .GetByIdAsync(id, x => x.Category, x => x.Photos);

                if (product is null)
                {
                    return BadRequest(new ResponseAPI(400));
                }

                return Ok(_mapper.Map<ProductDto>(product));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Add-Product")]
        public async Task<IActionResult> Add(AddProductDto addProductDto)
        {
            try
            {
                await unitOfWork.ProductRepository.AddAsync(addProductDto);

                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

        [HttpPut("Update-Product")]
        public async Task<IActionResult> Update(UpdateProductDto updateProductDto)
        {
            try
            {
                await unitOfWork.ProductRepository.UpdateAsync(updateProductDto);

                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

        [HttpDelete("delete-Product")]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                var product = await unitOfWork.ProductRepository.GetByIdAsync(id, x => x.Photos, x => x.Category); ;

                await unitOfWork.ProductRepository.DeleteAsync(product);

                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

    }
}
