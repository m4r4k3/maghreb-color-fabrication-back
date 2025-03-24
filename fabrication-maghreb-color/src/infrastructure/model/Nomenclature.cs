using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("NOMENCLATURE")]
    public class Nomenclature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }
        public string ReferencePf { get; set; }
        public string ReferenceMP { get; set; }
        public decimal Quantite { get; set; }
        public decimal Pourcentage { get; set; }
    }

}


