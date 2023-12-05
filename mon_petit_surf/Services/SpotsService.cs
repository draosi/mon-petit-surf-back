using Microsoft.EntityFrameworkCore;
using MonPetitSurf.Models;

namespace MonPetitSurf.Services
{
    public class SpotsService
    {
        private readonly MonPetitSurfContext _context;
        public SpotsService(MonPetitSurfContext context)
        {
            _context = context;
        }
        public async Task<List<Spots>> getSpots()
        {
            return await _context.Spots.ToListAsync();
        }

        public Spots getSpotById(int id)
        {
            var spot = _context.Spots.Where(e => e.Id == id).FirstOrDefault();
            return spot;
        }
    }
}
