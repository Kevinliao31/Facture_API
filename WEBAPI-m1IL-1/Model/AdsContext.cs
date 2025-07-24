using Microsoft.EntityFrameworkCore;

namespace WEBAPI_m1IL_1.Models
{
    public class AdsContext : DbContext
    {
        public AdsContext(DbContextOptions<AdsContext> options)
            : base(options)
        {
        }

        public DbSet<AdsItems> AdsItems { get; set; } = null!;
    }
}
