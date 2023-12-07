using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonPetitSurf.Dtos;
using MonPetitSurf.Models;
using MonPetitSurf.Services;

namespace MonPetitSurf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        protected UsersService _usersService { get; set; }

        public UsersController(UsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> registerUser(UserRegistrationDto registrationDto)
        {
            if (await _usersService.registerUser(registrationDto))
            {
                return Ok(new { message = "Nouvel utilisateur enregistré" });
            }

            return BadRequest("Echec de l'inscription");
        }

        [HttpPost("login")]
        public async Task<IActionResult> loginUser(UserDto userDto)
        {
            var user = await _usersService.getUserByUsername(userDto.Username);
            if (user == null)
            {
                return BadRequest("Nom d'utilisateur ou mot de passe incorrect");
            }

            if (BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
            {
                // Générez un jeton JWT
                var jwtToken = _usersService.generateJwtToken(user.Id);
                var response = new
                {
                    token = jwtToken,
                };

                return Ok(response);
            }

            return BadRequest("Nom d'utilisateur ou mot de passe incorrect");
        }

        [Authorize]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> getUserById(int id)
        {

            var userExist = await _usersService.getUserById(id);

            if (userExist == null)
            {
                return NotFound("Utilisateur non existant");
            }

            return Ok(userExist);
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> deleteUser(int id)
        {
            var user = await _usersService.userExist(id);

            if (!user)
            {
                return NotFound("Utilisateur non existant");
            }

            if (await _usersService.deleteUser(id))
            {
                return Ok(new {message = "Utilisitateur supprimé avec succès"});
            }

            return BadRequest("Échec de la suppression de l'utilisateur");
        }

        [Authorize]
        [HttpPut("put/{id}")]
        public async Task<IActionResult> updateUser(int id, [FromBody] UserDto user)
        {
            var userExist = await _usersService.getUserById(id);

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

            if (await _usersService.updateUser(userExist))
            {
                return Ok(new { message = "Utilisitateur modifié avec succès" });
            }

            return BadRequest("Échec de la modification de l'utilisateur");
        }

        [Authorize]
        [HttpGet("{id}/favorites")]
        public async Task<ActionResult<IEnumerable<UsersRegisterSpots>>> getUserFavorites(int id)
            => await _usersService.getUserFavorites(id);

        [Authorize]
        [HttpPost("{userId}/favorites/{spotId}")]
        public async Task<IActionResult> addFavorite(int userId, int spotId)
        {
            try
            {
                await _usersService.addFavorite(userId, spotId);
                return Ok("Spot ajouté aux favoris avec succès.");
            } catch (Exception ex)
            {
                return BadRequest($"Une erreur s'est produite : {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete("{userId}/favorites/{spotId}")]
        public async Task<IActionResult> deleteFavorite(int userId, int spotId)
        {
            try
            {
                await _usersService.deleteFavorite(userId, spotId);
                return Ok("Favoris supprimé avec succès.");
            } catch (Exception ex)
            {
                return BadRequest($"Une erreur s'est produite : {ex.Message}");
            }
        }
    }
}
