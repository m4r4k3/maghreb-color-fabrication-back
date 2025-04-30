using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;

namespace fabrication_maghreb_color.Infrastructure.model
{
    public class User
    {
        [Key]
        public Int32 Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        [JsonIgnore]

        public string Password { get; set; }

        [JsonIgnore]
        [Column("role")]
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        
        public Role role { get; set; }

        [JsonIgnore]
        public DateTime? CreatedAt { get; set; }
    }

}


