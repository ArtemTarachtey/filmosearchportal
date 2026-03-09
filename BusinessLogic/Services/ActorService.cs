using BusinessLogic.Interfaces;
using DataAccess.Context;
using Domain.DTO.ActorDTO;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class ActorService : IActorService
    {
        private readonly AppDbContext _dbContext;
        public ActorService(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<ActorReadDTO?> GetActorById(Guid id)
        {
            var actor =  await _dbContext.Actors.Include(a => a.Films).FirstOrDefaultAsync(a => a.Id == id);
            if (actor == null) return null;
            return new ActorReadDTO { Id = actor.Id, FirstName = actor.FirstName, LastName = actor.LastName, Age = actor.Age, Rating = actor.Rating, Films= actor.Films.Select(f => new FilmShortDTO { FilmId = f.Id, FilmTitle = f.Title }).ToList() };
        }

        public async Task<List<ActorReadDTO>> GetAllActors()
        {
            return await _dbContext.Actors.AsNoTracking().Select(a => new ActorReadDTO { Id = a.Id, FirstName = a.FirstName, LastName = a.LastName, Age = a.Age, Rating = a.Rating, Films = a.Films.Select(f => new FilmShortDTO { FilmId = f.Id, FilmTitle = f.Title }).ToList()}).ToListAsync();
        }

        public async Task<Guid> CreateActor(ActorCreateDTO createDTO)
        {
            var actor = new Actor
            {
                Id = Guid.NewGuid(),
                FirstName = createDTO.FirstName,
                LastName = createDTO.LastName,
                Age = createDTO.Age,
                Rating = createDTO.Rating,
                Films = new List<Film>()
            };

            if (createDTO.FilmIds?.Any() == true)
            {
                var films = await _dbContext.Films.Where(f => createDTO.FilmIds.Contains(f.Id)).ToListAsync();
                actor.Films = films;
            }
            _dbContext.Actors.Add(actor);
            await _dbContext.SaveChangesAsync();
            return actor.Id;
        }

        public async Task<bool> UpdateActor(ActorUpdateDTO updateDTO)
        {
            var actor = await _dbContext.Actors.Include(a => a.Films).FirstOrDefaultAsync(a => a.Id == updateDTO.Id);
            if (actor == null) return false;
            actor.FirstName = updateDTO.FirstName;
            actor.LastName = updateDTO.LastName;
            actor.Age = updateDTO.Age;
            actor.Rating = updateDTO.Rating;
            if (updateDTO.FilmIds != null)
            {
                actor.Films.Clear();
                var films = await _dbContext.Films.Where(f => updateDTO.FilmIds.Contains(f.Id)).ToListAsync();
                foreach (var film in films)
                {
                    actor.Films.Add(film);
                }
            }
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
        public async Task<Guid?> DeleteActor(Guid id)
        {
            var actor = await _dbContext.Actors.FindAsync(id);
            if (actor == null) return null;
            try
            {
                _dbContext.Actors.Remove(actor);
                await _dbContext.SaveChangesAsync();
                return actor.Id;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
