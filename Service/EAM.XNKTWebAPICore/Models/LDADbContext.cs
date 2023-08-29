using Microsoft.EntityFrameworkCore;

namespace EAM.XNKTWebAPICore.Models
{
    public partial class LDADbContext : DbContext
    {
        public LDADbContext()
        {
            
        }

        public LDADbContext(DbContextOptions<LDADbContext> options)
            : base(options)
        {
        }
    }
}
