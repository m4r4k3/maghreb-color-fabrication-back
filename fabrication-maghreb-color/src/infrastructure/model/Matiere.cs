using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("Matiere")]
    public class Matiere
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        [Required(ErrorMessage = "La référence de matière première est obligatoire.")]
        [StringLength(50, ErrorMessage = "La référence ne doit pas dépasser 50 caractères.")]
        public string ReferenceMP { get; set; }

        [Required(ErrorMessage = "Le TypeId est obligatoire.")]
        public int TypeId { get; set; }

        [Required(ErrorMessage = "La date d'affectation est obligatoire.")]
        public DateTime DateAffection { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La quantité utilisée est obligatoire.")]
        [Range(1, int.MaxValue, ErrorMessage = "La quantité utilisée doit être supérieure à zéro.")]
        public int QuantiteUtilise { get; set; }

        [Required(ErrorMessage = "Le pourcentage est obligatoire.")]
        [Range(0.01, 100.0, ErrorMessage = "Le pourcentage doit être entre 0.01 et 100.")]
        public decimal Pourcentage { get; set; }

        public int? Bon_id { get; set; }

        [ForeignKey("Bon_id")]
        public BonFabrication? Bon { get; set; }

        [ForeignKey("TypeId")]
        public virtual TypeMatiere? Type { get; set; }
    }
}
