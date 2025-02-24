using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using fabrication_maghreb_color.Config.Contexts;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace fabrication_maghreb_color.service
{
    public class UserService
    {
        private readonly MainContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(MainContext AppartementDbContextInjection, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = AppartementDbContextInjection;
            _configuration = configuration;
        }


        public bool ValidateToken()
        {
            try
            {
                string token = _httpContextAccessor.HttpContext.Request.Cookies["auth"];
                string SecretKey = _configuration["AppSettings:JWT_KEY"];
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(SecretKey);

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
            catch (Exception ex)
            {
                return false;
            }

        }
        public async Task<bool> checkUser(string username, string password)
        {
            var user = await _dbContext.UserDbo.FirstOrDefaultAsync(user => user.Username == username);

            if (user == null || BCrypt.Net.BCrypt.Verify(password, user.Password) == false)
            {
                return false;
            }
            return true;
        }

        public string UserToToken(string username)
        {
            string secret = _configuration["AppSettings:JWT_KEY"];
            var payload = new[]{
                new Claim(ClaimTypes.Name, username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
               claims: payload,
               expires: DateTime.Now.AddDays(1),
               signingCredentials: creds
           );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}