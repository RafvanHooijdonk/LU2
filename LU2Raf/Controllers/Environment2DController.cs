using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LU2Raf.Models;
using LU2Raf.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace LU2Raf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Environment2DController : ControllerBase
    {
        private readonly IEnvironment2DRepository _environmentRepo;
        private readonly IObject2DRepository _objectRepo; 

        public Environment2DController(IEnvironment2DRepository environmentRepo, IObject2DRepository objectRepo)
        {
            _environmentRepo = environmentRepo;
            _objectRepo = objectRepo;
        }

        [HttpPost("CreateUser")]
        public ActionResult CreateUser(User user)
        {
            UserStore.Users.Add(user);
            return CreatedAtAction(nameof(GetUser), new { username = user.Username }, user);
        }

        [HttpGet("GetUser/{username}")]
        public ActionResult GetUser(string username)
        {
            var user = UserStore.Users.FirstOrDefault(u => u.Username == username);
            return user == null ? NotFound("User not found") : Ok(user);
        }

        [HttpGet("GetUsers")]
        public ActionResult GetUsers()
        {
            return UserStore.Users.Count == 0 ? NotFound("No users found") : Ok(UserStore.Users);
        }

        [HttpPost("CreateEnvironment")]
        public async Task<ActionResult> CreateEnvironment2D(Environment2D environment)
        {
            await _environmentRepo.AddAsync(environment);
            return CreatedAtAction(nameof(CreateEnvironment2D), new { id = environment.Id }, environment);
        }

        [HttpGet("GetEnvironment/{id}")]
        public async Task<ActionResult> GetEnvironment2D(Guid Id)
        {
            var environment = await _environmentRepo.GetByIdAsync(Id);
            return environment == null ? NotFound("Environment not found") : Ok(environment);
        }

        [HttpGet("GetEnvironments")]
        public async Task<ActionResult> GetEnvironments()
        {
            var environments = await _environmentRepo.GetAllAsync();
            return Ok(environments);
        }

        [HttpPost("CreateObject")]
        public async Task<ActionResult> CreateObject2D(Object2D obj)
        {
            await _objectRepo.AddAsync(obj);
            return CreatedAtAction(nameof(CreateObject2D), new { id = obj.Id }, obj);
        }

        [HttpGet("GetObject/{id}")]
        public async Task<ActionResult> GetObject2D(Guid Id)
        {
            var obj = await _objectRepo.GetByIdAsync(Id);
            return obj == null ? NotFound("Object not found") : Ok(obj);
        }

        [HttpGet("GetObjects")]
        public async Task<ActionResult> GetObjects()
        {
            var objects = await _objectRepo.GetAllAsync();
            return Ok(objects);
        }
    }
}
