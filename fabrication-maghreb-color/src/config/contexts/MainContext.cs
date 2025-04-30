using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;

namespace fabrication_maghreb_color.Config.Contexts
{
    public class MainContext : DbContext
    {
        public DbSet<User> UserDbo { get; set; }
        public DbSet<BonFabrication> BonFabricationDbo { get; set; }
        public DbSet<PreparationFabrication> PreparationFabricationsDbo { get; set; }
        public DbSet<Matiere> MatiereDbo { get; set; }
        public DbSet<Projet> ProjetDbo { get; set; }
        public DbSet<TypeProjet> TypeProjetDbo { get; set; }
        public DbSet<BonFile> BonFileDbo { get; set; }
        public DbSet<TypeMatiere> TypeMatieresDbo { get; set; }
        public DbSet<Nomenclature> NomenclatureDbo { get; set; }
        public DbSet<Role> RoleDbo { get; set; }
        public DbSet<Policies> PoliciesDbo { get; set; }
        public DbSet<RolePolicies> RolePoliciesDbo { get; set; }
        private readonly IConfiguration? _configuration;
        public MainContext(DbContextOptions<MainContext> options, IConfiguration? configuration) : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RolePolicies>()
                .HasKey(rp => new { rp.RoleId, rp.PolicyId });
        }
    }
}