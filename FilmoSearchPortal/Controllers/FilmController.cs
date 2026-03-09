using BusinessLogic.Interfaces;
using Domain.DTO.FilmDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmoSearchPortal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FilmController: ControllerBase
    {
        private readonly IFilmService _filmService;
        public FilmController(IFilmService service) => _filmService = service;     

        [HttpGet] 
        public async Task<IActionResult> GetAllFilms()
        {
            var films = await _filmService.GetAllFilms();
            return Ok(films);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFilm(Guid id)
        {
            var film = await _filmService.GetFilmById(id);
            if (film == null) return NotFound("Film with this Id is not found!");
            return Ok(film);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFilm([FromBody] FilmCreateDTO filmDTO)
        {
            var id = await _filmService.CreateFilm(filmDTO);
            if (id == null) return BadRequest("Cannot create film!");
            return Ok(id);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateFilm([FromBody] FilmUpdateDTO filmDTO)
        {
            var isUpdate = await _filmService.UpdateFilm(filmDTO);
            if (isUpdate == null) return NotFound("Film with this Id is not found!");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilm(Guid id)
        {
            var deletedId = await _filmService.DeleteFilm(id);
            if (deletedId == null) return NotFound("Film with this Id is not found!");
            return NoContent();
        }
    }
}
