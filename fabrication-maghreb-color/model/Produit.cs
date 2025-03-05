using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fabrication_maghreb_color.model
{
    [Table("produit")]
    public class Produit
    {
        [Key]
        public Int32 Id { get; set; }
        public Decimal QuantiteDemande { get; set; }
        public string ReferencePf { get; set; }
        public DateTime? DateProduction { get; set; }

        [Column("id_projet")]
        public int ProjetId { get; set; }
    }

}


