using Microsoft.EntityFrameworkCore;
using MonPetiSurf.Context;
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
            return await _context.Spots.Select(e => e.Department).Distinct().ToListAsync();
        }
    }
}
