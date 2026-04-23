using BirdWatching.Common.Entities;
using BirdWatching.Interfaces;

namespace BirdWatching.DataPersistence.Repositories
{
    public class BirdWatchingRepositoryMemoryAsync : IBirdWatchingRepositoryAsync
    {
        private IList<Bird> _birds;

        public BirdWatchingRepositoryMemoryAsync()
        {
            _birds = new List<Bird>();
        }

        public async Task<BirdWatching.Common.Entities.Bird> AddAsync(BirdWatching.Common.Entities.Bird entity)
        {
            entity.Id = Guid.NewGuid();
            _birds.Add(entity);
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingEntity = await GetByIdAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException("Item with given id does not exist");
            }
            return _birds.Remove(existingEntity);
        }

        public async Task<System.Collections.Generic.IEnumerable<BirdWatching.Common.Entities.Bird>> GetAllAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentException("Expected page to be a positive number.");
            }
            if (pageSize <= 0)
            {
                throw new ArgumentException("Expected page to be a positive number.");
            }
            return _birds
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        public async Task<BirdWatching.Common.Entities.Bird?> GetByIdAsync(Guid id)
        {
            return _birds.FirstOrDefault(x => x.Id == id);
        }

        public async Task<bool> UpdateAsync(BirdWatching.Common.Entities.Bird updatedEntity)
        {
            var existingEntity = await GetByIdAsync(updatedEntity.Id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException("Item with given id does not exist");
            }
            existingEntity.Name = updatedEntity.Name;
            existingEntity.Order = updatedEntity.Order;
            existingEntity.Family = updatedEntity.Family;
            existingEntity.Habitat = updatedEntity.Habitat;
            existingEntity.PictureURL = updatedEntity.PictureURL;
            existingEntity.SightCount = updatedEntity.SightCount;

            return true;
        }
    }
}
