using BirdWatching.Common.Entities;

namespace BirdWatching.Interfaces
{
    public interface IEntityValidator<T> where T : IEntity
    {
        public void Validate(T entity);
    }
}
