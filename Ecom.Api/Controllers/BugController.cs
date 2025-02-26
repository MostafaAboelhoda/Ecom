using AutoMapper;
using Ecom.Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{
    public class BugController : BaseController
    {
        public BugController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        [HttpGet("not-found")]
        public async Task<IActionResult> GetNotFound()
        {
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(100);
            if (category is null)
                return NotFound();
            return Ok(category);

        }

        [HttpGet("server-error")]
        public async Task<IActionResult> GetServerError()
        {
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(100);
            category.Name = "";
            return Ok(category);
        }

        [HttpGet("bad-Request/{id}")]
        public async Task<IActionResult> GetBadRequest(int id)
        {
            return Ok();
        }

        [HttpGet("bad-Request")]
        public async Task<IActionResult> GetBadRequest()
        {
            return BadRequest();
        }

    }
}
