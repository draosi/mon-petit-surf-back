using Microsoft.EntityFrameworkCore;
using MonPetitSurf.Models;

namespace MonPetitSurf.Services
{
    public class UtilitiesService
    {
        private readonly MonPetitSurfContext _context;
        public UtilitiesService(MonPetitSurfContext context)
        {
            _context = context;
        }

        public async Task<List<Utilities>> getUtilities()
        {
            return await _context.Utilities.OrderBy(e => e.Title).ToListAsync();
        }

        public List<Utilities> getSpotUtilities(int spotId)
        {
            return _context.SpotsGetUtilities
                .Where(e => e.SpotId == spotId)
                .Select(e => e.Utility)
                .ToList();
        }

        public Utilities getUtilityById(int id)
        {
            return (_context.Utilities.Find(id));
        }

        public async Task postUtility(Spots spot, Utilities utility)
        {
            //var spot = getSpotById(model.SpotId);
            //var utility = getUtilityById(model.UtilityId);

            //if (spot != null && utility != null)
            //{
                var associate = new SpotsGetUtilities
                {
                    Spot = spot,
                    Utility = utility
                };

                _context.Add(associate);
                _context.SaveChanges();
            //}
            //else
            //{
            //    throw new Exception("L'equipement ou le spot n'existe pas");
            //}
        }

        public void deleteUtility(int spotId, int utilityId)
        {
            var utilityToDelete = _context.SpotsGetUtilities
                .SingleOrDefault(e => e.SpotId == spotId && e.UtilityId == utilityId);

            if (utilityToDelete != null)
            {
                _context.SpotsGetUtilities.Remove(utilityToDelete);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("L'equipement à effacer n'existe pas");
            }
        }
    }
}
