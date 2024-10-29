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
        [HttpGet("GetAll", Name = "Get All Expanses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ExpanseTrackerDto?>>> GetAllExpanses()
        {
            List<ExpanseTrackerDto?> expanseList = await ExpanseTracker.GetExpansesList();

            if (expanseList == null)
            {
                return NotFound("Expanses Not Found");
            }

            return Ok(expanseList);
        }
        [HttpGet("GetBy{ExpanseId}", Name = "GetExpanseById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ExpanseTrackerDto?>> GetExpanseById(int ExpanseId)
        {
            if (ExpanseId < 0)
            {
                return BadRequest($"Id {ExpanseId} less then 0");
            }

            ExpanseTracker? expanseTracker = await ExpanseTracker.FindExpanseById(ExpanseId);

            if (expanseTracker == null)
            {
                return NotFound("Expanse not found");
            }

            ExpanseTrackerDto dto = expanseTracker.Dto;

            return Ok(dto);
        }
        [HttpPost("AddNew", Name = "AddNewExpanse")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ExpanseTrackerDto?>> AddNewExpanse(ExpanseTrackerDto dto)
        {
            if (dto.Description == string.Empty || dto.Amount < 0)
            {
                return BadRequest("Bad request");
            }

            ExpanseTracker? expanseTracker = new ExpanseTracker(dto);

            if (await expanseTracker.Save())
            {
                dto.Id = expanseTracker.Id;
                return CreatedAtRoute("GetExpanseById", new { id = dto.Id }, dto);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
