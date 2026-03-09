using Domain.DTO.ActorDTO;

namespace BusinessLogic.Interfaces
{
    public interface IActorService
    {
        Task<Guid> CreateActor(ActorCreateDTO createDTO);
        Task<Guid?> DeleteActor(Guid id);
        Task<ActorReadDTO?> GetActorById(Guid id);
        Task<List<ActorReadDTO>> GetAllActors();
        Task<bool> UpdateActor(ActorUpdateDTO updateDTO);
    }
}