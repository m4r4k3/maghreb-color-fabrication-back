using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("F_COMPTET")]
    public class Compte
    {
        [Key]
        [Column("ct_num")]
        public string num { get; set; }
        [Column("ct_intitule")]

        public string intitule { get; set; }
        [Column("ct_type")]
        public Int16 type { get; set; }


    }
}

