using fabrication_maghreb_color.Infrastructure.model;

namespace fabrication_maghreb_color.Infrastructure.dto {
    public class PreparationRequest {
        public PreparationFabrication Document { get; set; }
        public decimal Quantite { get; set; }
    }
}