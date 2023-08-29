using Microsoft.EntityFrameworkCore;

namespace EAM.LDAWebAPICore.Models
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
