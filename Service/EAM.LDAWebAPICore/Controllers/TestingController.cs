using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EAM.LDAWebAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestingController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetAll()
        {
            return Ok("Request completed.");
        }
    }
}
