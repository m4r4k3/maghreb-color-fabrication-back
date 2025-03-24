using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("F_ENUMLIBRECIAL")]
    public class ChampLibre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cbmarq")]
        public Int32 Id { get; set; }
        [Column("EL_Intitule")]
        public string Label { get; set; }
        public Int16 N_File { get; set; }

    }
}

