using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace fabrication_maghreb_color.Infrastructure.model
{
    [Table("BonFiles")]
  public class BonFile
{
    [Key]
    public int Id { get; set; }
    public int BonId { get; set; }
    public string FilePath { get; set; } = null!;
    public DateTime UploadedAt { get; set; } = DateTime.Now;

    [ForeignKey("BonId")]
    public virtual BonFabrication Bon { get; set; } = null!;
}
}

