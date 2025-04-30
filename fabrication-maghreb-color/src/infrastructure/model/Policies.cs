using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("Policies")]
    public class Policies
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? BelongsTo { get; set; }

        [ForeignKey("BelongsTo")]
        [InverseProperty("Children")]
        public Policies Parent { get; set; }

        [InverseProperty("Parent")]
        public List<Policies> Children { get; set; }

        public List<RolePolicies> RolePolicies { get; set; }
    }
}
