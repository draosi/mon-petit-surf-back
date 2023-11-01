﻿using Microsoft.AspNetCore.Mvc;
using MonPetiSurf.Context;
using MonPetitSurf.Dtos;
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

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> deleteUser(int id)
        {
            var user = await _monPetitSurfService.userExist(id);

            if (!user)
            {
                return NotFound("Utilisateur non trouvé");
            }

            if (await _monPetitSurfService.deleteUser(id))
            {
                return Ok(new {message = "Utilisitateur supprimé avec succès"});
            }

            return BadRequest("Échec de la suppression de l'utilisateur");
        }
    }
}
