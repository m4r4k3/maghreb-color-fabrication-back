using fabrication_maghreb_color.Infrastructure.model;

namespace fabrication_maghreb_color.application.Interfaces
{
    public interface IRolePoliciesRepository
    {
        public Task Create(Policies policies);
    }
}