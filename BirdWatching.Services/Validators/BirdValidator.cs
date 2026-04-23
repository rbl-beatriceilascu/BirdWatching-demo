using BirdWatching.Common.Entities;
using BirdWatching.Common.Enums;
using BirdWatching.Interfaces;

namespace BirdWatching.Common.Validators
{
    public class BirdValidator : IEntityValidator<Bird>
    {
        public void Validate(Bird bird)
        {
            ArgumentNullException.ThrowIfNull(bird);

            if (string.IsNullOrWhiteSpace(bird.Name))
                throw new ArgumentException("Name is required");
            if (bird.Name.Length < 2)
                throw new ArgumentException("Length must be at least 2 characters");

            if (string.IsNullOrWhiteSpace(bird.Order))
                throw new ArgumentException("Order is required");
            if (bird.Order.Length < 2)
                throw new ArgumentException("Length must be at least 2 characters");

            if (string.IsNullOrWhiteSpace(bird.Family))
                throw new ArgumentException("Family is required");
            if (bird.Family.Length < 2)
                throw new ArgumentException("Length must be at least 2 characters");

            if (!Enum.IsDefined(typeof(Habitat), bird.Habitat))
                throw new ArgumentException("Habitat is required and must be a valid value");

            if (!string.IsNullOrEmpty(bird.PictureURL))
            {
                if (!Uri.TryCreate(bird.PictureURL, UriKind.Absolute, out _))
                    throw new ArgumentException("Picture URL is invalid");
            }

            if (bird.SightCount < 0)
                throw new ArgumentException("Sight count should be at least 0");
        }
    }
}
