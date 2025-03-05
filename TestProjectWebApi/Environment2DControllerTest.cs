//using System;
//using LU2Raf.Controllers;
//using LU2Raf.Models;
//using LU2Raf.Services;
//using LU2Raf.Repositories;
//using Microsoft.AspNetCore.Mvc;
//using Moq;

//namespace TestProjectWebApi
//{
//    [TestClass]
//    public class Environment2DControllerTest
//    {
//        private readonly Mock<IEnvironment2DRepository> _mockEnvironmentRepo;
//        private readonly Mock<IObject2DRepository> _mockObjectRepo;
//        private readonly Mock<IAuthenticationService> _mockAuthServiceRepo;
//        private readonly Environment2DController _controller;

//        public Environment2DControllerTest()
//        {
//            _mockEnvironmentRepo = new Mock<IEnvironment2DRepository>();
//            _mockObjectRepo = new Mock<IObject2DRepository>();
//            _mockAuthServiceRepo = new Mock<IAuthenticationService>();

//            _controller = new Environment2DController(
//                _mockEnvironmentRepo.Object,
//                _mockObjectRepo.Object,
//                (Microsoft.AspNetCore.Authentication.IAuthenticationService)_mockAuthServiceRepo.Object);
//        }

//        [TestMethod]
//        public async Task CreateEnvironment2D_ValidEnvironment_ReturnsOk()
//        {
//            // Arrange
//            var environment = new Environment2D("Test Environment", "user123", 10, 20);
//            _mockEnvironmentRepo.Setup(repo => repo.AddAsync(environment)).Returns(Task.CompletedTask);

//            // Act
//            var result = await _controller.CreateEnvironment2D(environment);

//            // Assert
//            Assert.IsInstanceOfType(result, typeof(OkResult));
//        }

//        [TestMethod]
//        public async Task GetEnvironment2D_ValidId_ReturnsEnvironment()
//        {
//            // Arrange
//            var environmentId = Guid.NewGuid();
//            var environment = new Environment2D("Test Environment", "user123", 10, 20) { Id = environmentId };
//            _mockEnvironmentRepo.Setup(repo => repo.GetByIdAsync(environmentId)).ReturnsAsync(environment);

//            // Act
//            var result = await _controller.GetEnvironment2D(environmentId) as OkObjectResult;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(environment, result.Value);
//        }

//        [TestMethod]
//        public async Task GetEnvironment2D_InvalidId_ReturnsNotFound()
//        {
//            // Arrange
//            var environmentId = Guid.NewGuid();
//            _mockEnvironmentRepo.Setup(repo => repo.GetByIdAsync(environmentId)).ReturnsAsync((Environment2D)null);

//            // Act
//            var result = await _controller.GetEnvironment2D(environmentId);

//            // Assert
//            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
//        }

//        [TestMethod]
//        public async Task GetEnvironments_ReturnsAllEnvironments()
//        {
//            // Arrange
//            var environments = new List<Environment2D>
//            {
//                new Environment2D("Environment 1", "user123", 10, 20),
//                new Environment2D("Environment 2", "user456", 15, 25)
//            };
//            _mockEnvironmentRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(environments);

//            // Act
//            var result = await _controller.GetEnvironments() as OkObjectResult;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(environments, result.Value);
//        }
//    }
//}
