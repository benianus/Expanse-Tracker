using ExpanseTrackerDataLayer;
using Microsoft.AspNetCore.Mvc;
using ExpanseTrackerBusinessLayer;

namespace ExpanseTrackerApiLayer.Controllers
{
    [Route("api/ExpanseTracker")]
    [ApiController]
    public class ExpanseTrackerApi : ControllerBase
    {
        [HttpGet("getAll", Name = "Get All Expanses")]
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
        [HttpGet("getBy{id}", Name = "GetExpanseById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ExpanseTrackerDto?>> GetExpanseById(int id)
        {
            if (id < 0)
            {
                return BadRequest($"Id {id} less then 0");
            }

            ExpanseTracker? expanseTracker = await ExpanseTracker.FindExpanseById(id);

            if (expanseTracker == null)
            {
                return NotFound("Expanse not found");
            }

            ExpanseTrackerDto dto = expanseTracker.Dto;

            return Ok(dto);
        }
        [HttpGet("GetSummary", Name = "GetExpansesSummary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetAllExpansesSummary()
        {
            int? summary = await ExpanseTracker.GetAllExpansesSummary();

            return Ok(summary);
        }
        [HttpGet("getSummaryBy{month}", Name = "GetExpansesSummaryByMonth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetExpansesSummaryByMonth(int month)
        {
            if (month < 0)
            {
                return BadRequest("Bad request, month wrong");
            }

            int? sum = await ExpanseTracker.GetExpansesSummaryByMonth(month);

            return Ok(sum);
        }
        [HttpGet("getExpansesBy{categoryId}", Name = "GetExpansesByCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ExpanseTrackerDto>>?> GetExpansesByCategory(int categoryId)
        {
            if (categoryId < 0 && categoryId > 7)
            {
                return BadRequest("Bad request, category id should be between 1 to 7");
            }

            List<ExpanseTrackerDto>? expanses = await ExpanseTracker.GetExpansesByCategory(categoryId);

            if (expanses == null)
            {
                return NotFound("Not found, No expanses found in this category");
            }

            return Ok(expanses);
        }
        [HttpPost("addNew", Name = "AddNewExpanse")]
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
                return CreatedAtRoute("GetExpanseById", new { Id = dto.Id }, dto);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        [HttpPut("update{id}", Name = "UpdateExpanseById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound) ]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateExpanse(int id, ExpanseTrackerDto dto)
        {
            if (id < 0 || dto.Description == string.Empty || dto.Amount < 0)
            {
                return BadRequest("Bad Request");
            }

            ExpanseTracker? updatedExpanse = await ExpanseTracker.FindExpanseById(id);

            if (updatedExpanse == null)
            {
                return NotFound($"Expanse with id {id} not found!");
            }
            
            updatedExpanse.Dto = dto;
            
            if (await updatedExpanse.Save())
            {
                return Ok("Expanse Updated successfully");
            }

            return StatusCode(StatusCodes.Status500InternalServerError); 
        }
        [HttpDelete("delete{id}", Name = "DeleteExpanseByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string?>> DeleteExpanseById(int id)
        {
            if (id < 0)
            {
                return BadRequest($"Bad Request, Id {id} less then 1");
            }
            
            ExpanseTracker? expanseToDelete = await ExpanseTracker.FindExpanseById(id);
                
            if (expanseToDelete == null)
            {
                return NotFound($"Expanse with id {id} not found!");
            }

            if (await expanseToDelete.DeleteExpanse(id))
            {
                return Ok("Expanse Deleted successfully");
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
