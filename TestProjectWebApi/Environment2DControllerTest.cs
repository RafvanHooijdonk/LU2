using System;
using LU2Raf.Controllers;
using LU2Raf.Models;
using LU2Raf.Services;
using LU2Raf.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TestProjectWebApi
{
    [TestClass]
    public class Environment2DControllerTest
    {
        private readonly Mock<IEnvironment2DRepository> _mockEnvironmentRepo;
        private readonly Mock<IObject2DRepository> _mockObjectRepo;
        private readonly Mock<IAuthenticationService> _mockAuthServiceRepo;
        private readonly Environment2DController _controller;

        public Environment2DControllerTest()
        {
            _mockEnvironmentRepo = new Mock<IEnvironment2DRepository>();
            _mockObjectRepo = new Mock<IObject2DRepository>();
            _mockAuthServiceRepo = new Mock<IAuthenticationService>();

            _controller = new Environment2DController(
                _mockEnvironmentRepo.Object,
                _mockObjectRepo.Object,
               _mockAuthServiceRepo.Object);
        }

        [TestMethod]
        public async Task CreateEnvironment2D_ValidEnvironment_ReturnsCreated()
        {
            var userId = Guid.NewGuid().ToString();
            var environment = new Environment2D("Test Environment", "user123", 10, 20) { Id = Guid.NewGuid() };

            _mockAuthServiceRepo.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(userId);
            _mockEnvironmentRepo.Setup(repo => repo.AddAsync(It.IsAny<Environment2D>())).Returns(Task.CompletedTask);

            var result = await _controller.CreateEnvironment2D(environment) as CreatedAtActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(_controller.CreateEnvironment2D), result.ActionName);
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            Assert.AreEqual(environment, result.Value);
        }

        [TestMethod]
        public async Task GetEnvironment2D_ValidId_ReturnsEnvironment()
        {
            var userId = Guid.NewGuid();
            var environment = new Environment2D("Test Environment", "user123", 10, 20) { Id = Guid.NewGuid() };
            var userEnvironments = new List<Environment2D> { environment };

            _mockAuthServiceRepo.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(userId.ToString());
            _mockEnvironmentRepo.Setup(repo => repo.GetByOwnerUserIdAsync(userId)).ReturnsAsync(userEnvironments);

            var result = await _controller.GetEnvironments() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(userEnvironments, result.Value);
        }

        [TestMethod]
        public async Task CreateObject2D_ValidObject_ReturnsCreated()
        {
            var obj = new Object2D
            {
                PrefabId = 5,
                PositionX = 5,
                PositionY = 10,
                ScaleX = 1,
                ScaleY = 1,
                RotationZ = 0,
                SortingLayer = 5,
                EnvironmentId = Guid.NewGuid().ToString()
            };

            _mockObjectRepo.Setup(repo => repo.AddAsync(It.IsAny<Object2D>())).Returns(Task.CompletedTask);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "user123")
            };
            var identity = new ClaimsIdentity(claims, "mock");
            var principal = new ClaimsPrincipal(identity);
            var context = new DefaultHttpContext { User = principal };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };

            var result = await _controller.CreateObject2D(obj) as CreatedAtActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(_controller.CreateObject2D), result.ActionName);
            Assert.AreEqual(obj, result.Value);
        }

        [TestMethod]
        public async Task GetObjects_ValidRequest_ReturnsObjects()
        {
            string environmentId = "test-environment-id"; 

            var objects = new List<Object2D>
            {
                new Object2D { Id = Guid.NewGuid(), PrefabId = 5, PositionX = 5, PositionY = 10, ScaleX = 1, ScaleY = 1, RotationZ = 0, SortingLayer = 5, EnvironmentId = environmentId },
                new Object2D { Id = Guid.NewGuid(), PrefabId = 5, PositionX = 15, PositionY = 20, ScaleX = 2, ScaleY = 2, RotationZ = 90, SortingLayer = 5, EnvironmentId = environmentId }
            };

            _mockObjectRepo.Setup(repo => repo.GetAllAsync(environmentId)).ReturnsAsync(objects);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "user123")
            };
            var identity = new ClaimsIdentity(claims, "mock");
            var principal = new ClaimsPrincipal(identity);
            var context = new DefaultHttpContext { User = principal };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };

            var result = await _controller.GetObjects(environmentId) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode); 
            Assert.AreEqual(objects, result.Value);
        }
    }
}
