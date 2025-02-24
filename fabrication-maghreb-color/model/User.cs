using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace fabrication_maghreb_color.model
{
    public class User
    {
        [Key]
        public Int32 Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

}


