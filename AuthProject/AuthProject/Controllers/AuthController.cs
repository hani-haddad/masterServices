using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthProject.ViewModels;
using AuthProject.Services;

namespace AuthProject.Controllers
{
    [Authorize]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        public IAuthService _auth;

        public AuthController(IAuthService authService)
        {
            this._auth = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserCredintials user)
        {
            UserClaims claims = await _auth.Login(user);
            if (claims != null)
            {
                return Ok(claims);
            }
            return Unauthorized(new { message = "Username or password is incorrect" });

        }
    }
}