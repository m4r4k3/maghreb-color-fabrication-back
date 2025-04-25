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
        [Column("DL_Design")]
        public string Designation { get; set; }
        [Column("dl_qte")]
        public decimal quantite { get; set; }
        [Column("dl_tnomencl")]
        public short isNomeclature { get; set; }
        [Column("DO_Piece")]
        public string DocumentNumber { get; set; }

        [Column("EU_Enumere")]
        public string Conditionnement { get; set; }

        [Column("DL_Taxe1")]
        public decimal Taxe1 { get; set; }

        [Column("DL_PUTTC")]
        public decimal PrixUnitaireTTC { get; set; }

        [Column("DL_MontantHT")]
        public decimal MontantHT { get; set; }

        [Column("DL_Remise01REM_Valeur")]
        public decimal RemiseValeur { get; set; }

        [ForeignKey("DocumentNumber")]
        public virtual Document Document { get; set; }
    }
}

