using Domain.DTO.FilmDTO;

namespace BusinessLogic.Interfaces
{
    public interface IFilmService
    {
        Task<Guid?> CreateFilm(FilmCreateDTO createDTO);
        Task<Guid?> DeleteFilm(Guid id);
        Task<List<FilmReadDTO>> GetAllFilms();
        Task<FilmReadDTO?> GetFilmById(Guid id);
        Task<bool?> UpdateFilm(FilmUpdateDTO updateDTO);
    }
}