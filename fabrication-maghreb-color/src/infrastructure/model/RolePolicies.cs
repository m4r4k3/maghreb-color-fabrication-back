using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("RolePolicies")]
    public class RolePolicies
    {
        [Key]
        [Column(Order = 0)]

        public int RoleId { get; set; }
        [Key]
        [Column(Order = 1)]

        public int PolicyId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role role { get; set; }
        [ForeignKey("PolicyId")]
        public virtual Policies policies { get; set; }
    }
}