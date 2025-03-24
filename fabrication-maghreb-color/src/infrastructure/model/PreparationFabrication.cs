using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("pf")]
    public class PreparationFabrication
    {
        [Key]
        public int Id { get; set; }
        public string? NumDocument { get; set; }
        public string ReferenceArticleSup { get; set; }

        [Required]
        public Decimal QuantiteEncre { get; set; }

        [Required]
        public Decimal QuantiteVernis { get; set; }

        public int? Projet_Id { get; set; }

        [ForeignKey("Projet_Id")]
        public virtual Projet? Projet { get; set; }
        [Column("machine")] 
        public int? MachineId { get; set; }
        [ForeignKey("MachineId")]
        public virtual ChampLibre? machine { get; set; }

        public virtual List<BonFabrication>? Bons { get; set; }

        public decimal? quantiteConsomme => Bons?.Sum(e => e.quantite) ?? 0;
    }
}
