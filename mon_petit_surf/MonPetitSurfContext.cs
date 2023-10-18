using Microsoft.EntityFrameworkCore;

namespace mon_petit_surf
{
    public class MonPetitSurfContext : DbContext
    {
        public MonPetitSurfContext(DbContextOptions<MonPetitSurfContext> options)
            : base(options)
        {
        }
    }
}
