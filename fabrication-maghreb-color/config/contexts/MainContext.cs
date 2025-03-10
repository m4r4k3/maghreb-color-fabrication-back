using fabrication_maghreb_color.model;
using Microsoft.EntityFrameworkCore;

namespace fabrication_maghreb_color.Config.Contexts
{
    public class MainContext : DbContext
    {
        public DbSet<User> UserDbo { get; set; }
        public DbSet<PreparationFabrication> PreparationFabricationsDbo { get; set; }
        public DbSet<Projet> ProjetDbo { get; set; }
        public DbSet<TypeProjet> typeProjetDbo { get; set; }
        public DbSet<Nomenclature> nomenclatureDbo {get; set;}
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