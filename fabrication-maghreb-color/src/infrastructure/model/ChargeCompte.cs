using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fabrication_maghreb_color.Infrastructure.model
{
       [Table("F_COLLABORATEUR")]

    public class chargeCompte
    {
        [Key]
        [Column("co_no")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }
        [Column("co_prenom")]
        public string prenom { get; set; }

        [Column("co_nom")]
        public string nom { get; set; }
    }

}


