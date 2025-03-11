using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LU2Raf.Models;
using LU2Raf.Repositories;
using LU2Raf.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace LU2Raf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Environment2DController : ControllerBase
    {
        private readonly IEnvironment2DRepository _environmentRepo;
        private readonly IObject2DRepository _objectRepo;
        private readonly Services.IAuthenticationService _authenticationService;

        public Environment2DController(IEnvironment2DRepository environmentRepo, IObject2DRepository objectRepo, Services.IAuthenticationService authenticationServiceRepo)
        {
            _environmentRepo = environmentRepo;
            _objectRepo = objectRepo;
            _authenticationService = authenticationServiceRepo;
        }

        [HttpPost("CreateUser")]
        [Authorize]
        public ActionResult CreateUser(User user)
        {
            UserStore.Users.Add(user);
            return CreatedAtAction(nameof(GetUser), new { username = user.Username }, user);
        }

        [HttpGet("GetUser/{username}")]
        [Authorize]
        public ActionResult GetUser(string username)
        {
            var user = UserStore.Users.FirstOrDefault(u => u.Username == username);
            return user == null ? NotFound("User not found") : Ok(user);
        }

        [HttpGet("GetUsers")]
        [Authorize]
        public ActionResult GetUsers()
        {
            return UserStore.Users.Count == 0 ? NotFound("No users found") : Ok(UserStore.Users);
        }

        [HttpPost("CreateEnvironment")]
        [Authorize]
        public async Task<ActionResult> CreateEnvironment2D(Environment2D environment)
        {
            var ownerUserId = _authenticationService.GetCurrentAuthenticatedUserId(); //User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (ownerUserId == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            environment.OwnerUserId = ownerUserId; 

            await _environmentRepo.AddAsync(environment);

            return CreatedAtAction(nameof(CreateEnvironment2D), new { id = environment.Id }, environment);
        }

        [HttpGet("GetEnvironments")]
        [Authorize]
        public async Task<ActionResult> GetEnvironments()
        {
            var ownerUserId = _authenticationService.GetCurrentAuthenticatedUserId();  //User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(ownerUserId) || !Guid.TryParse(ownerUserId, out Guid UserId))
            {
                return Unauthorized();
            }

            var environment = await _environmentRepo.GetByOwnerUserIdAsync(UserId);

            if (environment == null)
            {
                return NotFound("No environments found for this user.");
            }

            return Ok(environment);
        }

        [HttpPost("DeleteEnvironment")]
        [Authorize]
        public async Task<ActionResult> DeleteEnvironment2D([FromBody] Environment2D environment)
        {
            if (environment == null || string.IsNullOrEmpty(environment.Name))
            {
                return BadRequest("Ongeldige of ontbrekende environment naam.");
            }

            try
            {
                await _environmentRepo.DeleteAsync(environment.Name);
                return Ok($"Environment '{environment.Name}' en bijbehorende objecten zijn verwijderd.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Er is een fout opgetreden: {ex.Message}");
            }
        }

        [HttpPost("CreateObject")]
        [Authorize]
        public async Task<ActionResult> CreateObject2D(Object2D obj)
        {
            var ownerUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (ownerUserId == null)
            {
                return Unauthorized("User is not authenticated.");
            }
            if (obj.EnvironmentId == string.Empty)
            {
                return BadRequest("Object must be linked to a valid environment.");
            }
            await _objectRepo.AddAsync(obj);
            return CreatedAtAction(nameof(CreateObject2D), new { id = obj.Id }, obj);
        }

        [HttpGet("GetObjects")]
        [Authorize]
        public async Task<ActionResult> GetObjects(string environmentId)
        {
            var objects = await _objectRepo.GetAllAsync(environmentId);
            return Ok(objects);
        }
    }
}
