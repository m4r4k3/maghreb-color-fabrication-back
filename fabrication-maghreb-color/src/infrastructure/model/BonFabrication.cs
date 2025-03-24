using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("bf")]

    public class BonFabrication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }
        public string? NumPiece { get; set; }
        public decimal quantite { get; set; }
        public decimal QuantiteSupport {get ;set;}
        public DateTime DateCreation { get; set; } = DateTime.Now;

        public Int32 Pf_id { get; set; }
        [ForeignKey("Pf_id")]
        public virtual PreparationFabrication? preparationFabrication { get; set; }

    }
}

