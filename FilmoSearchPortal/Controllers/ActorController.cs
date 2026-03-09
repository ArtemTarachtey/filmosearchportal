using BusinessLogic.Interfaces;
using Domain.DTO.ActorDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmoSearchPortal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ActorController : ControllerBase
    {
        private readonly IActorService _actorService;
        public ActorController(IActorService service) => _actorService = service;

        [HttpGet]
        public async Task<IActionResult> GetAllActors()
        {
            var actors = await _actorService.GetAllActors();
            return Ok(actors);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActor([FromRoute] Guid id)
        {
            var actor = await _actorService.GetActorById(id);
            if (actor == null) return NotFound("Actor not found");
            return Ok(actor);
        }

        [HttpPost]
        public async Task<IActionResult> CreateActor([FromBody] ActorCreateDTO createDTO)
        {
            var id = await _actorService.CreateActor(createDTO);
            if (id == Guid.Empty) return BadRequest("Actor not create");
            return Ok(id);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateActor([FromBody] ActorUpdateDTO updateDTO)
        {
            var result = await _actorService.UpdateActor(updateDTO);
            if (!result) return NotFound("Actor with this id is not found"); 
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(Guid id)
        {
            var actorId = await _actorService.DeleteActor(id);
            if (actorId == null) return NotFound("Actor with this Id not found!");
            return NoContent();
        }
    }
}
