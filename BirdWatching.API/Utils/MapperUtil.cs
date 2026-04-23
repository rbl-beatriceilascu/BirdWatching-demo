using BirdWatching.API.Models;
using BirdWatching.Common.Entities;

namespace BirdWatching.API.Utils
{
    public static class MapperUtil
    {
        public static BirdModel toModel(this Bird bird)
        {
            return new BirdModel
            {
                Id = bird.Id,
                Name = bird.Name,
                Order = bird.Order,
                Family = bird.Family,
                Habitat = bird.Habitat,
                PictureURL = bird.PictureURL,
                SightCount = bird.SightCount,
            };
        }
        public static Bird toBird(this BirdModel model)
        {
            return new Bird
            {
                Id = model.Id,
                Name = model.Name,
                Order = model.Order,
                Family = model.Family,
                Habitat = model.Habitat,
                PictureURL = model.PictureURL,
                SightCount = model.SightCount,
            };
        }
    }
}
