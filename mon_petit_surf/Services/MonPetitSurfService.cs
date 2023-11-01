using Microsoft.EntityFrameworkCore;
using MonPetiSurf.Context;
using MonPetitSurf.Dtos;
using MonPetitSurf.Models;

namespace MonPetitSurf.Services
{
    public class MonPetitSurfService
    {
        private readonly MonPetitSurfContext _context;
        public MonPetitSurfService(MonPetitSurfContext context)
        {
            _context = context;
        }

        public async Task<List<Spots>> getSpots()
        {
            return await _context.Spots.ToListAsync();
        }

        public async Task<Spots?> getSpotById(int id)
        {
            return await _context.Spots.FindAsync(id);
        }

        public async Task<List<string>> getRegions()
        {
            return await _context.Spots
                .Select(e => e.Department)
                .Distinct()
                .OrderBy(e => e)
                .ToListAsync();
        }

        public async Task<List<Utilities>> getUtilities()
        {
            return await _context.Utilities.ToListAsync();
        }

        public async Task<bool> registerUser(UserRegistrationDto registrationDto)
        {
            if (isUserNameAvailable(registrationDto.Username))
            {
                return false;
            }

            string saltedPassword = BCrypt.Net.BCrypt.GenerateSalt(12);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password, saltedPassword);

            var user = new Users
            {
                Username = registrationDto.Username,
                Password = hashedPassword,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        private bool isUserNameAvailable(string userName)
        {
            return _context.Users.Any(e => e.Username == userName);
        }
    }
}
