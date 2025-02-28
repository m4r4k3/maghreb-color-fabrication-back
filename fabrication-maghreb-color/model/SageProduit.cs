using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fabrication_maghreb_color.model
{
    [Table("F_ARTICLE")]
    public class SageArticle
    {
        [Column("Ar_ref")]
        [Key]
        public string Reference { get;set; }

        [Column("Ar_design")]
        public string Designation { get; set; }

        [Column("Fa_codeFamille")]
        public string codeFamille { get; set; }

        [Column("AR_PrixTTC")]
        public Int16 PrixTTC { get; set; }
    }
}
