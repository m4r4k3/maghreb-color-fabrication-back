using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using fabrication_maghreb_color.Config.Contexts;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using fabrication_maghreb_color.Infrastructure.model;
using fabrication_maghreb_color.Infrastructure.Repositories;
using fabrication_maghreb_color.application.Interfaces;

namespace fabrication_maghreb_color.Application.Services
{
    public class UserService
    {
        private readonly MainContext _dbContext;
        private readonly IRolesRepository _rolesRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(MainContext AppartementDbContextInjection, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = AppartementDbContextInjection;
            _configuration = configuration;
        }

        public List<User> GetAllUsers()
        {
            return _dbContext.UserDbo.Include(e=>e.role).ToList();
        }
      
        public User? GetUser()
        {

            return _dbContext.UserDbo.FirstOrDefault(user => user.Username == _httpContextAccessor.HttpContext.User.Identity.Name);

        }
        public bool ValidateToken()
        {
            string token = _httpContextAccessor.HttpContext.Request.Cookies["auth"];
            string SecretKey = _configuration["AppSettings:JWT_KEY"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SecretKey);

            if (token == null) return false;
            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
                ValidateIssuer = false
            };
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            return true;


        }
        public async Task<(bool, User?)> checkUser(string username, string password)
        {
            User user = await _dbContext.UserDbo.Include(e => e.role).FirstOrDefaultAsync(user => user.Username == username);

            if (user == null || BCrypt.Net.BCrypt.Verify(password, user.Password) == false)
            {
                return (false, null);
            }
            return (true, user);
        }

        public string UserToToken(string username, string role)
        {
            string secret = _configuration["AppSettings:JWT_KEY"];
            var payload = new[]{
                new Claim(ClaimTypes.Name, username) ,
                new Claim(ClaimTypes.Role, role)

            };
            foreach (var claim in payload)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
               claims: payload,
               expires: DateTime.Now.AddDays(1),
               signingCredentials: creds
           );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public List<Policies> GetCurrentUserPermissions()
        {
            return _dbContext.UserDbo
                .Where(user => user.Username == _httpContextAccessor.HttpContext.User.Identity.Name)
                .SelectMany(user => user.role.rolePolicies)
                .Select(rolePolicy => rolePolicy.Policies)
                .ToList();
        }

    }
}