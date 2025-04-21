using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("pf")]
    public class PreparationFabrication
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Le numéro de document ne doit pas dépasser 50 caractères.")]
        public string? NumDocument { get; set; }

        [Required(ErrorMessage = "La référence d'article support est obligatoire.")]
        [StringLength(50, ErrorMessage = "La référence d'article support ne doit pas dépasser 50 caractères.")]
        public string ReferenceArticleSup { get; set; }

        [Required(ErrorMessage = "La quantité d'encre est obligatoire.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La quantité d'encre doit être supérieure à zéro.")]
        public decimal QuantiteEncre { get; set; }

        [Required(ErrorMessage = "La quantité de vernis est obligatoire.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La quantité de vernis doit être supérieure à zéro.")]
        public decimal QuantiteVernis { get; set; }

        public int? Projet_Id { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "La quantité support théorique doit être positive.")]
        public decimal? QuantiteSupportTheorique { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "La laize théorique doit être positive.")]
        public decimal? LaizeTheorique { get; set; }

        [ForeignKey("Projet_Id")]
        public virtual Projet? Projet { get; set; }

        public virtual List<BonFabrication>? Bons { get; set; }

        public decimal? quantiteConsomme => Bons?.Sum(e => e.quantite) ?? 0;
    }
}
