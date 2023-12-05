using Microsoft.EntityFrameworkCore;
using MonPetitSurf.Models;

namespace MonPetitSurf.Services
{
    public class RegionsService
    {
        private readonly MonPetitSurfContext _context;
        public RegionsService(MonPetitSurfContext context)
        {
            _context = context;
        }
        public async Task<List<string>> getRegions()
        {
            return await _context.Spots
                .Select(e => e.Department)
                .Distinct()
                .OrderBy(e => e)
                .ToListAsync();
        }
    }
}
