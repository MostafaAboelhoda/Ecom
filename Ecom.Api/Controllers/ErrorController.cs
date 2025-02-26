using Ecom.Api.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{
    [Route("errors/{statusCode}")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        public IActionResult Error(int statusCode)
        {
            return new ObjectResult(new ResponseAPI(statusCode));
        }
    }
}
