
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("DetailsOffset")]
    public class InformationOffset
    {
        [Key]
        public Int32 ProjetId { get; set; }
        public decimal Largeur { get; set; }
        public decimal Longueur { get; set; }
        public string Papier { get; set; }
        public int Pose { get; set; }
        public int Grammage { get; set; }
    }
}

