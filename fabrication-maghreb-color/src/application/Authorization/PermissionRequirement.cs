using Microsoft.AspNetCore.Authorization;

namespace fabrication_maghreb_color.application.Authorization
{

    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }

}