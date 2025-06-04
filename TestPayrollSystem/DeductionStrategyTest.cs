using backend.Domain;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace backend.API.Tests
{
    [TestFixture]
    public class APIControllerTests
    {
        private APIController _controller;
        private Mock<IAPIRepository> _mockRepo;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IAPIRepository>();
            var mockConfig = new Mock<IConfiguration>();
            _controller = new APIController(mockConfig.Object)
            {
                // Inyectamos el mock del repositorio (necesitarías cambiar el controlador para permitir esto)
                _repo = _mockRepo.Object
            };
        }

        [Test]
        public void GetAPIs_ReturnsListOfApis()
        {
            // Arrange
            var expectedApis = new List<ApiModel>
            {
                new ApiModel { Id = Guid.NewGuid(), Name = "API 1" },
                new ApiModel { Id = Guid.NewGuid(), Name = "API 2" }
            };

            _mockRepo.Setup(repo => repo.GetAPIs()).Returns(expectedApis);

            // Act
            var result = _controller.GetAPIs();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedApis, okResult.Value);
        }

        [Test]
        public void GetParameters_WithValidApiId_ReturnsParameters()
        {
            // Arrange
            var apiId = Guid.NewGuid();
            var expectedParameters = new List<ApiParameterModel>
            {
                new ApiParameterModel { Id = Guid.NewGuid(), ApiId = apiId, Name = "Param 1" },
                new ApiParameterModel { Id = Guid.NewGuid(), ApiId = apiId, Name = "Param 2" }
            };

            _mockRepo.Setup(repo => repo.GetParametersByAPI(apiId)).Returns(expectedParameters);

            // Act
            var result = _controller.GetParameters(apiId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedParameters, okResult.Value);
        }

        [Test]
        public void AddParameterValue_WithValidValue_ReturnsTrue()
        {
            // Arrange
            var newValue = new ParameterValueModel
            {
                ParameterId = Guid.NewGuid(),
                Value = "Test Value",
                Timestamp = DateTime.Now
            };

            _mockRepo.Setup(repo => repo.AddParameterValue(It.IsAny<ParameterValueModel>())).Returns(true);

            // Act
            var result = _controller.AddParameterValue(newValue);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsTrue((bool)okResult.Value);
        }

        [Test]
        public void AddParameterValue_WithNullValue_ReturnsBadRequest()
        {
            // Arrange
            ParameterValueModel nullValue = null;

            // Act
            var result = _controller.AddParameterValue(nullValue);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Parameter value is null", badRequestResult.Value);
        }
    }
}