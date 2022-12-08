using Microsoft.AspNetCore.Mvc;

namespace jorgen.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JorgenController : ControllerBase
    {
        private readonly ILogger _logger;
        public JorgenController(ILogger<JorgenController> logger)
        {
            _logger = logger;   
        }

        [HttpGet]
        public IActionResult GetJorgenImage()
        {
            _logger.LogInformation("Starting API CALL", DateTimeOffset.UtcNow);
            try
            {
                string? imagepathName = AppDomain.CurrentDomain.BaseDirectory + "Data\\jorgenimg.png" ?? null;

                if (imagepathName == null)
                {
                    _logger.LogInformation("Image is null", DateTimeOffset.UtcNow);
                    return Content("Could not find the image of Jorgen");
                }

                byte[] b = System.IO.File.ReadAllBytes(imagepathName);

                _logger.LogInformation("Sending image", DateTimeOffset.UtcNow);
                return File(b, "image/png");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error message: {e.Message}", DateTimeOffset.UtcNow);
                return Content($"Could not GET image.{Environment.NewLine} Error message: {e.Message} ");
            }
        }
    }
}