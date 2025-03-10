using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fabrication_maghreb_color.model
{
    [Table("PreparationFabrication")]
    public class PreparationFabrication
    {
        [Key]
        public int NumPiece { get; set; }

        [Required]
        public int Quantite { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrixR { get; set; }

        public int? Projet_Id { get; set; }

        [ForeignKey("Projet_Id")]
        public Projet? Projet { get; set; }
    }
}