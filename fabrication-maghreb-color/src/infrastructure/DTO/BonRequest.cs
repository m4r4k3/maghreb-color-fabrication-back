using fabrication_maghreb_color.Infrastructure.model;

namespace fabrication_maghreb_color.Infrastructure.dto
{
    public class BonRequest
    {
        public BonFabrication bon { get; set; }
        public List<Matiere> matieres { get; set; }
        public List<IFormFile> files { get; set; } = new List<IFormFile>();
    }
}