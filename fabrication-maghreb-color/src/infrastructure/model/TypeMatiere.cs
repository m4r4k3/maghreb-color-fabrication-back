using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fabrication_maghreb_color.Infrastructure.model
{

    [Table("typeMatiere")]
    public class TypeMatiere
    {
        [Key]
        public Int32 Id { get; set; }
        public string Label { get; set; }

    }

}


