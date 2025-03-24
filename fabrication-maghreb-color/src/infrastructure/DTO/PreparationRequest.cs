using fabrication_maghreb_color.Infrastructure.model;

namespace FabricationMaghrebColor.Infrastructure.DTO {
    public class PreparationRequest {
        public PreparationFabrication Document { get; set; }
        public decimal Quantite { get; set; }
    }
}