using ExpanseTrackerDataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ExpanseTrackerApiLayer.Controllers
{
    [Route("api/ExpanseTracker")]
    [ApiController]
    public class ExpanseTrackerApi : ControllerBase
    {
        [HttpGet("GetAll" , Name = "Get All Expanses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetAllExpanses()
        {
            return Ok("Done");
        }
    }
}
