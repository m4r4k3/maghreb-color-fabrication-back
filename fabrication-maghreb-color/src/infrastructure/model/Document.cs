using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("F_DOCENTETE")] // Assuming the table is called F_DOCENTETE, which is common in Sage-like structures
    public class Document
    {
        [Key]
        [Column("DO_Piece")]
        public string DocumentNumber { get; set; }

        [Column("DO_Ref")]
        public string DocumentReference { get; set; }

        [Column("DO_Date")]
        public DateTime DocumentDate { get; set; }

        [Column("DO_Tiers")]
        public string? CustomerCode { get; set; }

        [Column("DO_Statut")]
        public short DocumentStatus { get; set; }

        [Column("DO_Type")]
        public short DocumentType { get; set; }

        public virtual List<DocLigne>? DocumentLines { get; set; } 
        [Column("co_no")]
        public int? ChargeCompteId { get; set; }

        [ForeignKey("ChargeCompteId")]
        public virtual chargeCompte? ChargeCompte { get; set; }


        [ForeignKey("CustomerCode")]
        public virtual Compte? Customer { get; set; }

    }
}
