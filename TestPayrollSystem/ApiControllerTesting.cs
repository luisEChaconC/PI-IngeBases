using backend.API;
using backend.Domain;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TestPayrollSystem
{
    [TestFixture]
    public class APIControllerTest
    {
        private APIController _controller;
        private Mock<APIRepository> _repoMock;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<APIRepository>();
            _controller = new APIController(_repoMock.Object);
        }

        [Test]
        public void GetAPIs_ReturnsListOfApis()
        {
            var expected = new List<ApiModel> { new ApiModel { Name = "Test API" } };
            _repoMock.Setup(r => r.GetAPIs()).Returns(expected);

            var result = (OkObjectResult)_controller.GetAPIs();

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expected, result.Value);
        }

        [Test]
        public void AddParameterValue_NullInput_ReturnsBadRequest()
        {
            var result = _controller.AddParameterValue(null);

            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }
    }
}
