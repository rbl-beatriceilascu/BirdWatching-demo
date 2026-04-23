using BirdWatching.API.Models;
using BirdWatching.API.Utils;
using BirdWatching.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BirdWatching.API.Controllers
{
    [Route("api/v1/birds")]
    [ApiController]
    public class BirdWatchingController : ControllerBase
    {
        private IBirdWatchingServiceAsync _birdWatchingService;
        public BirdWatchingController(IBirdWatchingServiceAsync birdWatchingServiceAsync)
        {
            _birdWatchingService = birdWatchingServiceAsync;
        }

        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BirdModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");
            var result = await _birdWatchingService.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result.toModel());
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<BirdModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] int? pageNumber, int? pageSize)
        {
            var results = await _birdWatchingService.GetAllAsync(pageNumber, pageSize);
            if (!results.Any())
                return NoContent();
            return Ok(results.Select(x => x.toModel()));
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BirdModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add([FromBody] BirdModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _birdWatchingService.AddAsync(model.toBird());
            return Ok(result.toModel());
        }

        [HttpPut]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] BirdModel model)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            if (id != model.Id)
                return BadRequest("Ids do not match");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _birdWatchingService.UpdateAsync(model.toBird());
            if (result)
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);

        }

        [HttpDelete]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = await _birdWatchingService.DeleteAsync(id);

            if (result) return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

    }
}
