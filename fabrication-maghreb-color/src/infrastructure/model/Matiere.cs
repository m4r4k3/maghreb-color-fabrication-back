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

        public string ReferenceMP { get; set; }
        public int type { get; set; }
        public DateTime DateAffection { get; set; }
        public int QuantiteUtilise { get; set; }
        public decimal Pourcentage {get ; set; }
        public int? Bon_id { get; set; }
        [ForeignKey("Bon_id")]
        public BonFabrication? Bon { get; set; }
    }

}


