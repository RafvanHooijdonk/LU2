using System;
using LU2Raf.Controllers;
using LU2Raf.Models;
using LU2Raf.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProjectWebApi
{
    [TestClass]
    public class Environment2DControllerTest
    {
        private readonly Mock<IEnvironment2DRepository> _mockEnvironmentRepo;
        private readonly Mock<IObject2DRepository> _mockObjectRepo;
        private readonly Environment2DController _controller;

        public Environment2DControllerTest()
        {
            _mockEnvironmentRepo = new Mock<IEnvironment2DRepository>();
            _mockObjectRepo = new Mock<IObject2DRepository>();
            _controller = new Environment2DController(_mockEnvironmentRepo.Object, _mockObjectRepo.Object);
        }

        [TestMethod]
        public async Task CreateEnvironment2D_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var environment = new Environment2D("Test Environment", 10, 20);

            // Act
            var result = await _controller.CreateEnvironment2D(environment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            var actionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult.Value, typeof(Environment2D));
            var returnValue = actionResult.Value as Environment2D;
            Assert.AreEqual(environment.Name, returnValue.Name);
        }

        [TestMethod]
        public async Task GetEnvironment2D_ReturnsEnvironment_WhenEnvironmentExists()
        {
            // Arrange
            var environmentId = Guid.NewGuid();
            var environment = new Environment2D("Test Environment", 10, 20) { Id = environmentId };
            _mockEnvironmentRepo.Setup(repo => repo.GetByIdAsync(environmentId)).ReturnsAsync(environment);

            // Act
            var result = await _controller.GetEnvironment2D(environmentId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var actionResult = result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult.Value, typeof(Environment2D));
            var returnValue = actionResult.Value as Environment2D;
            Assert.AreEqual(environmentId, returnValue.Id);
        }

        [TestMethod]
        public async Task GetEnvironment2D_ReturnsNotFound_WhenEnvironmentDoesNotExist()
        {
            // Arrange
            var environmentId = Guid.NewGuid();
            _mockEnvironmentRepo.Setup(repo => repo.GetByIdAsync(environmentId)).ReturnsAsync((Environment2D)null);

            // Act
            var result = await _controller.GetEnvironment2D(environmentId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetEnvironments_ReturnsAllEnvironments()
        {
            // Arrange
            var environments = new List<Environment2D>
                    {
                        new Environment2D("Environment 1", 10, 20),
                        new Environment2D("Environment 2", 15, 25)
                    };
            _mockEnvironmentRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(environments);

            // Act
            var result = await _controller.GetEnvironments();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var actionResult = result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult.Value, typeof(List<Environment2D>));
            var returnValue = actionResult.Value as List<Environment2D>;
            Assert.AreEqual(2, returnValue.Count);
        }

        [TestMethod]
        public async Task CreateObject2D_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var obj = new Object2D("Prefab1", 1.0f, 2.0f, 1.0f, 1.0f, 0.0f, 0);

            // Act
            var result = await _controller.CreateObject2D(obj);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            var actionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult.Value, typeof(Object2D));
            var returnValue = actionResult.Value as Object2D;
            Assert.AreEqual(obj.PrefabId, returnValue.PrefabId);
        }

        [TestMethod]
        public async Task GetObject2D_ReturnsObject_WhenObjectExists()
        {
            // Arrange
            var objectId = Guid.NewGuid();
            var obj = new Object2D("Prefab1", 1.0f, 2.0f, 1.0f, 1.0f, 0.0f, 0) { Id = objectId };
            _mockObjectRepo.Setup(repo => repo.GetByIdAsync(objectId)).ReturnsAsync(obj);

            // Act
            var result = await _controller.GetObject2D(objectId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var actionResult = result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult.Value, typeof(Object2D));
            var returnValue = actionResult.Value as Object2D;
            Assert.AreEqual(objectId, returnValue.Id);
        }

        [TestMethod]
        public async Task GetObject2D_ReturnsNotFound_WhenObjectDoesNotExist()
        {
            // Arrange
            var objectId = Guid.NewGuid();
            _mockObjectRepo.Setup(repo => repo.GetByIdAsync(objectId)).ReturnsAsync((Object2D)null);

            // Act
            var result = await _controller.GetObject2D(objectId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetObjects_ReturnsAllObjects()
        {
            // Arrange
            var objects = new List<Object2D>
                    {
                        new Object2D("Prefab1", 1.0f, 2.0f, 1.0f, 1.0f, 0.0f, 0),
                        new Object2D("Prefab2", 3.0f, 4.0f, 1.5f, 1.5f, 45.0f, 1)
                    };
            _mockObjectRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(objects);

            // Act
            var result = await _controller.GetObjects();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var actionResult = result as OkObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult.Value, typeof(List<Object2D>));
            var returnValue = actionResult.Value as List<Object2D>;
            Assert.AreEqual(2, returnValue.Count);
        }
    }
}
