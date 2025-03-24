using fabrication_maghreb_color.Infrastructure.model;

namespace FabricationMaghrebColor.Infrastructure.DTO
{
    public class BonRequest
    {
        public BonFabrication bon { get; set; }
        public List<Matiere> matieres { get; set; }
    }
}