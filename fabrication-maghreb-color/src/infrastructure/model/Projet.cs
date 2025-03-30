using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("Projet")]
    public class Projet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }
        public string Intitule { get; set; }
        public string? Description { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public DateTime DateLivraisonPrev { get; set; }
        public string ReferenceClient { get; set; }
        public string? ReferenceArticle { get; set; }
        public string? NumeroDossier { get; set; }
        [Column("chargeCompte")]
        public int chargeCompteId { get; set; }
        public int TypeDescription { get; set; } = 0;
        [Column("id_typeprojet")]
        public int TypeProjet { get; set; }
        public Decimal? quantite { get; set; }
        public virtual PreparationFabrication? preparationFabrication { get; set; }
        [ForeignKey("TypeProjet")]
        public virtual TypeProjet? Type { get; set; }
        [ForeignKey("chargeCompteId")]
        public virtual chargeCompte? ChargeCompte { get; set; }
        public bool hasPreparation => preparationFabrication != null;
    }
}
