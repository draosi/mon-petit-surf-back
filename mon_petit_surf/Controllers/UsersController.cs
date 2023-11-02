using Microsoft.AspNetCore.Mvc;
using MonPetiSurf.Context;
using MonPetitSurf.Dtos;
using MonPetitSurf.Models;
using MonPetitSurf.Services;

namespace MonPetitSurf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly MonPetitSurfContext _context;
        protected MonPetitSurfService _monPetitSurfService { get; set; }

        public UsersController(MonPetitSurfContext context)
        {
            _context = context;
            _monPetitSurfService = new MonPetitSurfService(_context);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> registerUser(UserRegistrationDto registrationDto)
        {
            if (await _monPetitSurfService.registerUser(registrationDto))
            {
                return Ok(new { message = "Nouvel utilisateur enregistré" });
            }

            return BadRequest("Echec de l'inscription");
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> getUserById(int id)
        {
            var userExist = await _monPetitSurfService.getUserById(id);

            if (userExist == null)
            {
                return NotFound("Utilisateur non existant");
            }

            return Ok(userExist);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> deleteUser(int id)
        {
            var user = await _monPetitSurfService.userExist(id);

            if (!user)
            {
                return NotFound("Utilisateur non existant");
            }

            if (await _monPetitSurfService.deleteUser(id))
            {
                return Ok(new {message = "Utilisitateur supprimé avec succès"});
            }

            return BadRequest("Échec de la suppression de l'utilisateur");
        }

        [HttpPut("put/{id}")]
        public async Task<IActionResult> updateUser(int id, [FromBody] UserUpdateDto user)
        {
            var userExist = await _monPetitSurfService.getUserById(id);

            if (userExist == null)
            {
                return NotFound("Utilisateur non existant");
            }

            if(!string.IsNullOrEmpty(user.Username))
            {
                userExist.Username = user.Username;
            }
            if(!string.IsNullOrEmpty(user.Password))
            {
                string saltedPassword = BCrypt.Net.BCrypt.GenerateSalt(12);
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, saltedPassword);
                userExist.Password = hashedPassword;
            }

            if (await _monPetitSurfService.updateUser(userExist))
            {
                return Ok(new { message = "Utilisitateur modifié avec succès" });
            }

            return BadRequest("Échec de la modification de l'utilisateur");
        }

        [HttpGet("{id}/favorites")]
        public async Task<ActionResult<IEnumerable<UsersRegisterSpots>>> getUserFavorites(int id)
            => await _monPetitSurfService.getUserFavorites(id);
    }
}
