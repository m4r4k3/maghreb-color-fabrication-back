using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("F_DOCLIGNE")]
    public class DocLigne
    {
        [Key]
        public string Do_piece { get; set; }
        [Column("ar_ref")]
        public string Article { get; set; }
        [Column("dl_qte")]
        public double quantite { get; set; }
        [Column("dl_tnomencl")]
        public string isNomeclature { get; set; }
    }
}

