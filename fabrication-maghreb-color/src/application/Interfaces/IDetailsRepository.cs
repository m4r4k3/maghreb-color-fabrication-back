using fabrication_maghreb_color.Infrastructure.model;

namespace fabrication_maghreb_color.application.Interfaces
{
    public interface  IDetailsRepository
    {
        Task InsertDetailsOffset (InformationOffset details);
    }
}