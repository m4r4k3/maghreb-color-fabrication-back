using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using fabrication_maghreb_color.Infrastructure.model;
using fabrication_maghreb_color.Infrastructure.dto;
using fabrication_maghreb_color.Application.Services;

namespace fabrication_maghreb_color.api.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(UserService userService, ILogger<UserController> logger)
        {
            _logger = logger;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("check")]
        public IActionResult check()
        {
            try
            {
                return Ok(new
                {
                    isLoggedIn = _userService.ValidateToken(),
                });
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "error",
                    message = "Une erreur s'est produite lors de la vérification du statut de connexion."
                });
            }
        }

        [HttpGet("me")]
        public IActionResult me()
        {
            try
            {
                return Ok(new
                {
                    status = "success",
                    data = _userService.GetUser()
                });
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "error",
                    message = "Une erreur s'est produite lors de la récupération des informations de l'utilisateur."
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginModel loginModel)
        {
            try
            {
                var (check, user) = await _userService.checkUser(loginModel.username, loginModel.password);
                if (check)
                {
                    HttpContext.Response.Cookies.Append("auth", _userService.UserToToken(user.Username, user.Role), new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.Now.AddDays(1),
                        SameSite = SameSiteMode.None
                    });
                    return Ok(new
                    {
                        status = "success",
                        message = "Connexion réussie",
                    });
                }

                return Unauthorized(new
                {
                    status = "error",
                    message = "Nom d'utilisateur ou mot de passe invalide",
                    details = "Le nom d'utilisateur ou le mot de passe fourni ne correspond pas à nos enregistrements. Veuillez réessayer."
                });
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "error",
                    message = "Une erreur s'est produite lors de la tentative de connexion."
                });
            }
        }

        [HttpGet("logout")]
        public IActionResult logout()
        {
            try
            {
                HttpContext.Response.Cookies.Delete("auth");
                return Ok(new
                {
                    status = "success",
                    message = "Déconnexion réussie"
                });
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);;
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "error",
                    message = "Une erreur s'est produite lors de la déconnexion."
                });
            }
        }
    }
}
