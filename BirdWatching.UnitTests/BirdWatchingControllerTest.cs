using BirdWatching.API.Controllers;
using BirdWatching.API.Models;
using BirdWatching.API.Utils;
using BirdWatching.Common.Entities;
using BirdWatching.Common.Enums;
using BirdWatching.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BirdWatching.UnitTests
{
    public class BirdWatchingControllerTest
    {
        private readonly Mock<IBirdWatchingServiceAsync> _mockService;
        private readonly BirdWatchingController _controller;

        public BirdWatchingControllerTest()
        {
            _mockService = new Mock<IBirdWatchingServiceAsync>();
            _controller = new BirdWatchingController(_mockService.Object);
        }

        [Fact]
        public async Task Get_WithValidId_ReturnsOkWithBird()
        {
            var birdId = Guid.NewGuid();
            var fakeBird = new Bird
            {
                Id = birdId,
                Name = "TestBird"
            };
            _mockService
                .Setup(s => s.GetByIdAsync(birdId))
                .ReturnsAsync(fakeBird);

            var result = await _controller.Get(birdId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var birdResult = (BirdModel)okResult.Value;
            var fakeBirdModel = fakeBird.toModel();
            Assert.Equal(birdResult.Name, fakeBirdModel.Name);
        }

        [Fact]
        public async Task Get_WithEmptyId_ReturnsBadRequestWithErrorMessage()
        {
            var emptyId = Guid.Empty;

            var result = await _controller.Get(emptyId);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Id cannot be empty", badRequestResult.Value);
            _mockService.Verify(s => s.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task Get_WithNonexistentId_ReturnsNotFound()
        {
            var birdId = Guid.NewGuid();
            _mockService
                .Setup(s => s.GetByIdAsync(birdId))
                .ReturnsAsync((Bird?)null);

            var result = await _controller.Get(birdId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Get_WhenNoBirdExists_ReturnsNoContent()
        {
            _mockService
                .Setup(s => s.GetAllAsync(null, null))
                .ReturnsAsync(new List<Bird>());

            var result = await _controller.Get(null, null);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Get_WithPaging_ReturnsOkWithResultEnumerable()
        {
            int pageNumber = 1;
            int pageSize = 1;
            var birdId = Guid.NewGuid();
            Bird fakeBird = new Bird
            {
                Id = birdId,
            };
            _mockService
                .Setup(s => s.GetAllAsync(pageNumber, pageSize))
                .ReturnsAsync(new List<Bird> { fakeBird });

            var result = await _controller.Get(pageNumber, pageSize);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var birdList = (IEnumerable<BirdModel>)okResult.Value;
            Assert.Equal(birdList.Count(), pageSize);
            var fakeBirdModel = fakeBird.toModel();
            var firstBird = birdList.First();
            Assert.NotNull(firstBird);
            Assert.Equal(firstBird.Id, birdId);
        }

        [Fact]
        public async Task Add_WithValidBird_ReturnsOkWithAddedBird()
        {
            var newBird = new BirdModel
            {
                Name = "Azure Whisperfinch",
                Order = "Passeriformes",
                Family = "Fringillidae",
                Habitat = Habitat.Forest,
                PictureURL = "https://example.com/images/azure-whisperfinch.jpg",
                SightCount = 12
            };
            var savedBird = new Bird
            {
                Id = Guid.NewGuid(),
                Name = newBird.Name,
                Family = newBird.Family,
                Habitat = newBird.Habitat,
                PictureURL = newBird.PictureURL,
                SightCount = newBird.SightCount
            };
            _mockService
                .Setup(s => s.AddAsync(It.IsAny<Bird>()))
                .ReturnsAsync(savedBird);

            var result = await _controller.Add(newBird);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            _mockService.Verify(x => x.AddAsync(It.IsAny<Bird>()), Times.Once);
            var birdResult = (BirdModel)okResult.Value;
            Assert.Equal(newBird.Name, birdResult.Name);
        }

        [Fact]
        public async Task Update_WithIdMismatch_ReturnsBadRequestWithErrorMessage()
        {
            var routeId = Guid.NewGuid();
            var model = new BirdModel
            {
                Id = Guid.NewGuid(),
                Name = "Azure Whisperfinch",
                Order = "Passeriformes",
                Family = "Fringillidae",
                Habitat = Habitat.Forest,
                PictureURL = "https://example.com/images/azure-whisperfinch.jpg",
                SightCount = 12
            };

            var result = await _controller.Update(routeId, model);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Ids do not match", badRequest.Value);
            _mockService.Verify(s => s.UpdateAsync(It.IsAny<Bird>()), Times.Never);
        }

        [Fact]
        public async Task Update_WithEmptyId_ReturnsBadRequestWithErrorMessage()
        {
            var emptyId = Guid.Empty;
            var model = new BirdModel
            {
                Id = Guid.Empty,
                Name = "Azure Whisperfinch",
                Order = "Passeriformes",
                Family = "Fringillidae",
                Habitat = Habitat.Forest,
                PictureURL = "https://example.com/images/azure-whisperfinch.jpg",
                SightCount = 12
            };

            var result = await _controller.Update(emptyId, model);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Id cannot be empty", badRequest.Value);
            _mockService.Verify(s => s.UpdateAsync(It.IsAny<Bird>()), Times.Never);
        }

        [Theory]
        [InlineData(true, StatusCodes.Status200OK)]
        [InlineData(false, StatusCodes.Status500InternalServerError)]
        public async Task Update_WithServiceResult_ReturnsExpectedStatusCode(bool serviceResult, int expectedStatusCode)
        {
            var routeId = Guid.NewGuid();
            var model = new BirdModel
            {
                Id = routeId,
                Name = "Azure Whisperfinch",
                Order = "Passeriformes",
                Family = "Fringillidae",
                Habitat = Habitat.Forest,
                PictureURL = "https://example.com/images/azure-whisperfinch.jpg",
                SightCount = 12
            };
            _mockService
                .Setup(s => s.UpdateAsync(It.IsAny<Bird>()))
                .ReturnsAsync(serviceResult);

            var result = await _controller.Update(routeId, model);

            var statusCode = (result as OkResult)?.StatusCode
                             ?? (result as StatusCodeResult)?.StatusCode;
            Assert.Equal(expectedStatusCode, statusCode);
        }

        [Theory]
        [InlineData(true, StatusCodes.Status200OK)]
        [InlineData(false, StatusCodes.Status500InternalServerError)]
        public async Task Delete_WithServiceResult_ReturnsExpectedStatusCode(bool serviceResult, int expectedStatusCode)
        {
            var birdId = Guid.NewGuid();
            _mockService
                .Setup(s => s.DeleteAsync(birdId))
                .ReturnsAsync(serviceResult);

            var result = await _controller.Delete(birdId);

            var statusCode = (result as OkResult)?.StatusCode
                             ?? (result as StatusCodeResult)?.StatusCode;
            Assert.Equal(expectedStatusCode, statusCode);
        }

        [Fact]
        public async Task Delete_WithEmptyId_ReturnsBadRequestWithErrorMessage()
        {
            var emptyId = Guid.Empty;

            var result = await _controller.Get(emptyId);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Id cannot be empty", badRequestResult.Value);
            _mockService.Verify(s => s.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}
