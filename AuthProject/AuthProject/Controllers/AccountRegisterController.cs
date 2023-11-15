using System;
using System.Collections.Generic;
using AuthProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SharedModelNamespace.Shared;
namespace AuthProject.Controllers
{[
    Authorize]
    [Route("api/register")]
    [ApiController]
   public class AccountRegisterController : Controller
   {
        public readonly IAuthService _auth;

        public AccountRegisterController(IAuthService authService)
        {
            this._auth = authService;
        }

        [AllowAnonymous]
       [HttpPost("register-new-user")]
       [HttpPost]
       public async Task<IActionResult> Create(User user)
       {
            var isCreated  = await _auth.Create(user);
            if (isCreated == null)
                return Ok("Username Added:" + user.Username);
            else 
                return NotFound(new { message = isCreated});
       }

   }
}