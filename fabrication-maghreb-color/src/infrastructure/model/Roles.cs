using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Label { get; set; }
        public virtual List<RolePolicies> rolePolicies {get ;set;} 
    }
}