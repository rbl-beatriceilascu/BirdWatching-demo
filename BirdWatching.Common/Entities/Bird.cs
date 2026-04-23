using BirdWatching.Common.Enums;

namespace BirdWatching.Common.Entities
{
    public class Bird : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Order { get; set; } = string.Empty;
        public string Family { get; set; } = string.Empty;
        public Habitat Habitat { get; set; }
        public string PictureURL { get; set; } = string.Empty;
        public int SightCount { get; set; } = 0;
    }
}
