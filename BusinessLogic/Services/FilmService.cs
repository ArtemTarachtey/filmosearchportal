using BusinessLogic.Interfaces;
using DataAccess.Context;
using Domain.DTO.FilmDTO;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class FilmService : IFilmService
    {
        private readonly AppDbContext _dbContext;

        public FilmService(AppDbContext context) => _dbContext = context;

        public async Task<FilmReadDTO?> GetFilmById(Guid id)
        {
            return await _dbContext.Films.AsNoTracking()
                .Where(f => f.Id == id)
                .Select(f => new FilmReadDTO
                {
                    Id = f.Id,
                    Title = f.Title,
                    Description = f.Description,
                    Duration = f.Duration,
                    Actors = f.Actors.Select(a => new ActorShortDTO
                    {
                        actorId = a.Id,
                        actorName = a.FirstName + " " + a.LastName,
                    }).ToList(),
                    Reviews = f.Reviews.Select(r => new ReviewShortDTO
                    {
                        reviewId = r.Id,
                        reviewName = r.Title
                    }).ToList(),
                }).FirstOrDefaultAsync();
        }

        public async Task<List<FilmReadDTO>> GetAllFilms()
        {
            return await _dbContext.Films.AsNoTracking()
                .Select(f =>
                new FilmReadDTO
                {
                    Id = f.Id,
                    Title = f.Title,
                    Description = f.Description,
                    Duration = f.Duration,
                    Actors = f.Actors.Select(a => new ActorShortDTO { actorId = a.Id, actorName = a.FirstName + " " + a.LastName}).ToList(),
                    Reviews = f.Reviews.Select(r => new ReviewShortDTO
                    {
                        reviewId = r.Id,
                        reviewName = r.Title
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<Guid?> CreateFilm(FilmCreateDTO createDTO)
        {
            if (createDTO == null) return null;
            var film = new Film
            {
                Id = Guid.NewGuid(),
                Title = createDTO.Title,
                Description = createDTO.Description,
                Duration = createDTO.Duration
            };
            if (createDTO.ActorIds != null && createDTO.ActorIds.Any())
            {
                var actors = await _dbContext.Actors.Where(a => createDTO.ActorIds.Contains(a.Id)).ToListAsync();
                film.Actors = actors;
            }
            try
            {
                _dbContext.Films.Add(film);
                await _dbContext.SaveChangesAsync();
                return film.Id;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool?> UpdateFilm(FilmUpdateDTO updateDTO)
        {
            var film = await _dbContext.Films.AsSplitQuery().Include(a => a.Actors).Include(r => r.Reviews).FirstOrDefaultAsync(f => f.Id == updateDTO.Id);
            if (film == null) return null;
            film.Title = updateDTO.Title;
            film.Description = updateDTO.Description;
            film.Duration = updateDTO.Duration;

            if (updateDTO.ActorIds != null)
            {
                film.Actors.Clear();
                var actors = await _dbContext.Actors.Where(a => updateDTO.ActorIds.Contains(a.Id)).ToListAsync();
                foreach (var actor in actors)
                {
                    film.Actors.Add(actor);
                }
            }
            //if (updateDTO.ReviewIds != null)
            //{
            //    film.Reviews.Clear();
            //    var reviews = await _dbContext.Reviews.Where(r => updateDTO.ReviewIds.Contains(r.Id)).ToListAsync();
            //    foreach (var review in reviews)
            //    {
            //        film.Reviews.Add(review);
            //    }
            //}
            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Guid?> DeleteFilm(Guid id)
        {
            var film = await _dbContext.Films.FindAsync(id);
            if (film == null) return null;
            try
            {
                _dbContext.Films.Remove(film);
                await _dbContext.SaveChangesAsync();
                return film.Id;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
