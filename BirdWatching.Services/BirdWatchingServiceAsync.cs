using BirdWatching.Common.Entities;
using BirdWatching.Interfaces;

namespace BirdWatching.Services
{
    public class BirdWatchingServiceAsync : IBirdWatchingServiceAsync
    {
        private const int DefaultPageNumber = 1;
        private const int DefaultPageSize = 10;
        private IBirdWatchingRepositoryAsync _birdRepository;
        private IEntityValidator<Bird> _birdValidator;

        public BirdWatchingServiceAsync(IBirdWatchingRepositoryAsync birdRepository, IEntityValidator<Bird> birdValidator)
        {
            _birdRepository = birdRepository;
            _birdValidator = birdValidator;
        }

        public async Task<BirdWatching.Common.Entities.Bird> AddAsync(BirdWatching.Common.Entities.Bird entity)
        {
            _birdValidator.Validate(entity);
            return await _birdRepository.AddAsync(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _birdRepository.DeleteAsync(id);
        }

        public async Task<System.Collections.Generic.IEnumerable<BirdWatching.Common.Entities.Bird>> GetAllAsync(int? pageNumber, int? pageSize)
        {
            pageNumber = pageNumber ?? DefaultPageNumber;
            pageSize = pageSize ?? DefaultPageSize;

            if (pageNumber <= 0)
            {
                throw new ArgumentException("Expected page to be a positive number.");
            }
            if (pageSize <= 0)
            {
                throw new ArgumentException("Expected page to be a positive number.");
            }
            return await _birdRepository.GetAllAsync(pageNumber.Value, pageSize.Value);
        }

        public async Task<BirdWatching.Common.Entities.Bird?> GetByIdAsync(Guid id)
        {
            return await _birdRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(BirdWatching.Common.Entities.Bird updatedEntity)
        {
            _birdValidator.Validate(updatedEntity);
            return await _birdRepository.UpdateAsync(updatedEntity);
        }
    }
}
