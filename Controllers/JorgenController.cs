using Microsoft.AspNetCore.Mvc;

namespace jorgen.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JorgenController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetJorgenImage()
        {
            try
            {
                string? imagepathName = AppDomain.CurrentDomain.BaseDirectory + "data\\jorgen.png" ?? null;

                if (imagepathName == null)
                {
                    return Content("Could not find the image of Jorgen");
                }

                byte[] b = System.IO.File.ReadAllBytes(imagepathName);

                return File(b, "image/png");
            }
            catch (Exception e)
            {
                return Content($"Could not GET image. {Environment.NewLine} Error message: {e.Message} ");
            }
        }
    }
}