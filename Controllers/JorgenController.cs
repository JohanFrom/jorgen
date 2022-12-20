using jorgen.Models;
using jorgen.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace jorgen.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JorgenController : ControllerBase
    {
        private readonly IJorgenService _jorgenService; 

        public JorgenController(IJorgenService jorgenService)
        {
            _jorgenService = jorgenService;
        }

        /// <summary>
        /// Retrives an image of Jorgen
        /// </summary>
        /// <returns>image.png</returns>
        [HttpGet("jorgenimage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetJorgenImage()
        {
            
            byte[] bytes = _jorgenService.GetJorgenImage();

            if (bytes == null)
            {
                return Content("Could not find image");
            }

            return File(bytes, "image/png");
        }

        /// <summary>
        /// Returns the status of Jorgens Beard based on the temperature
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        [HttpGet("statusOfBeard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<string> GetJorgenStatusBeard(double temp)
        {
            string strOut = _jorgenService.GetBeardStatus(temp);

            if(strOut == null)
            {
                return NotFound();
            }

            return strOut;
        }
    }
}
