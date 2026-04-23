using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BirdWatching.Common.Enums;

namespace BirdWatching.API.Models
{
    public class BirdModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "Length must be at least 2 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Order is required")]
        [MinLength(2, ErrorMessage = "Length must be at least 2 characters")]
        public string Order { get; set; } = string.Empty;

        [Required(ErrorMessage = "Family is required")]
        [MinLength(2, ErrorMessage = "Length must be at least 2 characters")]
        public string Family { get; set; } = string.Empty;

        [Required(ErrorMessage = "Habitat is required")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Habitat Habitat { get; set; }

        [UrlAttribute(ErrorMessage = "Picture URL is invalid")]
        public string PictureURL { get; set; } = string.Empty;

        [Range(0, Int32.MaxValue, ErrorMessage = "Sight count should be at least 0")]
        public int SightCount { get; set; } = 0;
    }
}
