using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using fabrication_maghreb_color.Infrastructure.model;
using fabrication_maghreb_color.application.service;

namespace fabrication_maghreb_color.api.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("check")]
        public IActionResult check()
        {
            return Ok(new
            {
                isLoggedIn = _userService.ValidateToken(),
            });
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginModel loginModel)
        {
            var (check , user) = await _userService.checkUser(loginModel.username, loginModel.password);
            if (check)
            {
                HttpContext.Response.Cookies.Append("auth", _userService.UserToToken(user.Username , user.Role), new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.Now.AddDays(1),
                    SameSite = SameSiteMode.None
                });
                return Ok(new
                {
                    status = "success",
                    message = "Login successful",
                }
);
            }

            return Unauthorized(new
            {
                status = "error",
                message = "Invalid username or password",
                details = "The username or password provided does not match our records. Please try again."
            }
);
        }
        [HttpGet("logout")]
        public IActionResult logout()
        {
            HttpContext.Response.Cookies.Delete("auth");
            return Ok(new
            {
                status = "success",
                message = "Logout successful"
            });
        }
    }
}
