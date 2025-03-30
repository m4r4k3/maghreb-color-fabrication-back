using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;

namespace fabrication_maghreb_color.Config.Contexts
{
    public class MainContext : DbContext
    {
        public DbSet<User> UserDbo { get; set; }
        public DbSet<BonFabrication> BonFabricationDbo { get; set; }
        public DbSet<PreparationFabrication> PreparationFabricationsDbo { get; set; }
        public DbSet<chargeCompte> ChargeCompteDbo { get; set; }
        public DbSet<Matiere> MatiereDbo { get; set; }
        public DbSet<Projet> ProjetDbo { get; set; }
        public DbSet<TypeProjet> typeProjetDbo { get; set; }
        private readonly IConfiguration? _configuration;
        public MainContext(DbContextOptions<MainContext> options, IConfiguration? configuration) : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}