using Microsoft.EntityFrameworkCore;
using Bushmakina.Domain.Entities;

namespace Bushmakina.Domain.Infrastructure
{
    public class PartnersDbContext : DbContext
    {
        public DbSet<PartnerEntity> Partners { get; set; }
        public DbSet<PartnerTypeEntity> PartnerTypes { get; set; }
        public DbSet<SalesRecordEntity> SalesRecords { get; set; }

        public PartnersDbContext(DbContextOptions<PartnersDbContext> options) : base(options) { }
    }
}