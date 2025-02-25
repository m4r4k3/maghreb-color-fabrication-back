using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fabrication_maghreb_color.model
{
    [Table("Projet")]
    public class Projet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }
        public string Intitule { get; set; }
        public string Description { get; set; }
        public DateTime? DateDebut
        { get; set; }
        public DateTime? DateFin { get; set; }
        public string ReferenceClient { get; set; }
        [Column("id_typeprojet")]
        public int TypeProjet { get; set; }
    }

}


