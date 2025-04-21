using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using fabrication_maghreb_color.application.repository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("bf")]

    public class BonFabrication : IValidatableObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }
        public decimal quantite { get; set; }
        public decimal QuantiteSupport { get; set; }
        public string Laize { get; set; }
        public string MetrageLineaire { get; set; }
        public string Passe { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.Now;

        public Decimal? QuantiteFini { get; set; }
        public Decimal? NombreBobins { get; set; }
        public Boolean Fini { get; set; } = false;
        public Boolean Deleted { get; set; } = false;
        [Column("machine")]
        public int? MachineId { get; set; }
        [NotMapped]
        public ChampLibre? machine { get; set; }
        public Int32 Pf_id { get; set; }
        [ForeignKey("Pf_id")]
        public virtual PreparationFabrication? preparationFabrication { get; set; }
        public virtual List<Matiere>? matieres { get; set; }
        public virtual List<BonFile>? files { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (quantite <= 0)
            {
                yield return new ValidationResult("La quantité doit être supérieure à zéro.", new[] { nameof(quantite) });
            }
            if (QuantiteSupport <= 0)
            {
                yield return new ValidationResult("La quantité de support doit être supérieure à zéro.", new[] { nameof(QuantiteSupport) });
            }
            if (string.IsNullOrWhiteSpace(Laize))
            {
                yield return new ValidationResult("La laize ne peut pas être vide.", new[] { nameof(Laize) });
            }
            if (string.IsNullOrWhiteSpace(MetrageLineaire))
            {
                yield return new ValidationResult("Le métrage linéaire ne peut pas être vide.", new[] { nameof(MetrageLineaire) });
            }
            if (string.IsNullOrWhiteSpace(Passe))
            {
                yield return new ValidationResult("Le passe ne peut pas être vide.", new[] { nameof(Passe) });
            }
            if (QuantiteFini <= 0)
            {
                yield return new ValidationResult("La quantité finie ne peut pas être inférieure à zéro.", new[] { nameof(QuantiteFini) });
            }


        }
    }
}

