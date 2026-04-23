using BirdWatching.Common.Entities;

namespace BirdWatching.Interfaces
{
    public interface IBirdWatchingServiceAsync
    {
        Task<Bird?> GetByIdAsync(Guid id);
        Task<IEnumerable<Bird>> GetAllAsync(int? pageNumber, int? pageSize);
        Task<Bird> AddAsync(Bird entity);
        Task<bool> UpdateAsync(Bird updatedEntity);
        Task<bool> DeleteAsync(Guid id);
    }
}
