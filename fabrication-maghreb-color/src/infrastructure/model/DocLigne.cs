using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("F_DOCLIGNE")]
    public class DocLigne
    {

        [Column("cbMarq")]
        [Key]
        public int Id { get; set; }

        [Column("ar_ref")]
        public string Article { get; set; }
        [Column("dl_qte")]
        public decimal quantite { get; set; }
        [Column("dl_tnomencl")]
        public short isNomeclature { get; set; }
        [Column("DO_Piece")]
        public string DocumentNumber { get; set; }
        [ForeignKey("DocumentNumber")]
        public virtual Document Document { get; set; }
    }
}

