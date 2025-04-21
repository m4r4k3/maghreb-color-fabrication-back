using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("Projet")]
    public class Projet : IValidatableObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }
        public string Intitule { get; set; }
        public string? Description { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public DateTime? DateLivraisonPrev { get; set; }
        public string ReferenceClient { get; set; }
        public string? ReferenceArticle { get; set; }
        public Boolean HasNomenclature { get; set; } = false;

        public string? NumeroDossier { get; set; }
        public int TypeDescription { get; set; } = 0;

        public Decimal? quantite { get; set; }

        [Column("chargeCompte")]
        public int chargeCompteId { get; set; }
        [Column("id_typeprojet")]
        public int TypeProjet { get; set; }
        public virtual PreparationFabrication? preparationFabrication { get; set; }
        [ForeignKey("TypeProjet")]
        public virtual TypeProjet? Type { get; set; }
        [ForeignKey("chargeCompteId")]
        public virtual chargeCompte? ChargeCompte { get; set; }
        public virtual List<Nomenclature>? Nomenclatures { get; set; }
        public bool hasPreparation => preparationFabrication != null;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateFin.HasValue && DateFin < DateDebut)
                yield return new ValidationResult("La date de fin ne peut pas être antérieure à la date de début.", new[] { nameof(DateFin) });
            if (DateLivraisonPrev.HasValue && DateLivraisonPrev < DateDebut)
                yield return new ValidationResult("La date de livraison prévue ne peut pas être antérieure à la date de début.", new[] { nameof(DateLivraisonPrev) });
        }
    }
}
