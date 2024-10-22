using ExpanseTrackerDataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ExpanseTrackerBusinessLayer;

namespace ExpanseTrackerApiLayer.Controllers
{
    [Route("api/ExpanseTracker")]
    [ApiController]
    public class ExpanseTrackerApi : ControllerBase
    {
        [HttpGet("GetAll" , Name = "Get All Expanses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ExpanseTrackerDto>>> GetAllExpanses()
        {
            List<ExpanseTrackerDto> expanseList = await ExpanseTracker.GetExpansesList();

            if (expanseList == null)
            {
                return NotFound("Expanses Not Found");
            }

            return Ok(expanseList);
        }
    }
}
