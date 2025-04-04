using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;

namespace fabrication_maghreb_color.Config.Contexts
{
    public class SageContext : DbContext
    {
        public DbSet<SageArticle> SageArticleDbo { get; set; }
        public DbSet<DocLigne> DocLigneDbo { get; set; }
        private DbSet<ChampLibre> _ChampLibreDbo { get; set; }
        public IQueryable<ChampLibre> Machine => _ChampLibreDbo.Where(c => c.N_File == 35);
        public DbSet<Compte> ComptetDbo { get; set; }
        private readonly IConfiguration? _configuration;
        public SageContext(DbContextOptions<SageContext> options, IConfiguration? configuration) : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SageArticle>()
       .HasKey(sp => sp.Reference);

            // Configure your entity mappings here
            // modelBuilder.Entity<YourEntity>().ToTable("YourTableName");
        }
    }
}