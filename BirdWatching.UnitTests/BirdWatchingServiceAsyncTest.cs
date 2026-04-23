using BirdWatching.Common.Entities;
using BirdWatching.Common.Enums;
using BirdWatching.Interfaces;
using BirdWatching.Services;
using Moq;

namespace BirdWatching.UnitTests
{
    public class BirdWatchingServiceAsyncTest
    {
        private readonly Mock<IBirdWatchingRepositoryAsync> _mockRepository;
        private readonly Mock<IEntityValidator<Bird>> _mockValidator;
        private readonly BirdWatchingServiceAsync _service;

        public BirdWatchingServiceAsyncTest()
        {
            _mockRepository = new Mock<IBirdWatchingRepositoryAsync>();
            _mockValidator = new Mock<IEntityValidator<Bird>>();
            _service = new BirdWatchingServiceAsync(_mockRepository.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task AddAsync_WithValidBird_ReturnsBird()
        {
            var bird = new Bird
            {
                Name = "TestBird",
                Order = "Passeriformes",
                Family = "Fringillidae",
                Habitat = Habitat.Forest,
                PictureURL = "http://image.com",
                SightCount = 1
            };
            var savedBird = new Bird
            {
                Id = Guid.NewGuid(),
                Name = bird.Name
            };
            _mockRepository
                .Setup(r => r.AddAsync(bird))
                .ReturnsAsync(savedBird);

            var result = await _service.AddAsync(bird);

            Assert.NotNull(result);
            Assert.Equal(savedBird.Id, result.Id);
            _mockValidator.Verify(v => v.Validate(bird), Times.Once);
            _mockRepository.Verify(r => r.AddAsync(bird), Times.Once);
        }

        [Fact]
        public async Task AddAsync_WhenValidationFails_ThrowsException_AndDoesNotCallRepository()
        {
            var bird = new Bird();
            _mockValidator
                .Setup(v => v.Validate(bird))
                .Throws(new ArgumentException("Invalid bird"));

            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAsync(bird));

            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Bird>()), Times.Never);
        }
    }
}
