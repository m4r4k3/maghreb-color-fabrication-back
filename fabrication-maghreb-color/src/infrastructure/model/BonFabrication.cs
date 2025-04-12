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
        public decimal QuantiteSupport { get; set; }
        public string Laize { get; set; }
        public string MetrageLineaire { get; set; }
        public string Passe { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.Now;
        [Column("machine")]
        public int? MachineId { get; set; }
        [ForeignKey("MachineId")]
        public ChampLibre? machine { get; set; }
        public Int32 Pf_id { get; set; }
        [ForeignKey("Pf_id")] 
        public virtual PreparationFabrication? preparationFabrication { get; set; }
        public virtual List<Matiere>? matieres { get; set; }
        public virtual List<BonFile>? files { get; set; }

}
}

