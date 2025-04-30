using System.Security.Claims;
using fabrication_maghreb_color.application.Interfaces;
using fabrication_maghreb_color.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace fabrication_maghreb_color.application.Authorization
{

    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IRolesRepository _rolesRepository;

        public PermissionHandler(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var roleClaim = context.User.FindFirst(ClaimTypes.Role)?.Value;
            Console.WriteLine("roleClaim :" + roleClaim) ;
            if (string.IsNullOrEmpty(roleClaim))
                return;

            // Get permissions for this role from the repository
            var permissions = _rolesRepository.GetRolesWithInclusion(roleClaim).rolePolicies.Select(rp => rp.policies)
        .ToList(); ;

            if (permissions.Any(e => e.Name == requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }

}