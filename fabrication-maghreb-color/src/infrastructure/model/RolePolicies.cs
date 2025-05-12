using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("RolePolicies")]
    public class RolePolicies
    {
        [Key]
        public int Id { get; set; }

        public int RoleId { get; set; }

        public int PolicyId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }

        [ForeignKey("PolicyId")]
        public virtual Policies? Policies { get; set; }
    }

}