using System.ComponentModel.DataAnnotations;

namespace fabrication_maghreb_color.model
{
    public class Produit
    {
        [Key]
        public Int32 Id { get; set; }
        public Decimal QuantiteDemande { get; set; }
        public string ReferencePf { get; set; }
        public DateTime? DateProduction { get; set; }

        public int IdProjet { get; set; }
    }

}


