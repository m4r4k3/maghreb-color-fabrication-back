using System.ComponentModel.DataAnnotations;

namespace fabrication_maghreb_color.model
{
    public class Matiere
    {
        [Key]
        public Int32 Id { get; set; }
        public DateTime? DateAffectation { get; set; }
        public bool ParUnite { get; set; }
        public string ReferenceMP { get; set; }
        public Decimal QuantiteUtilise { get; set; }
        public Decimal Pourcentage { get; set; }
        public int IdProjet { get; set; }
    }

}


