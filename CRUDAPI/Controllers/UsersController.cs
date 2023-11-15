using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using SharedModelNamespace.Shared;
using SharedModelNamespace.Shared.ViewModels;
using CRUDAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CRUDAPI.Controllers
{
    [Authorize]
    //class to perform CRUD operations
    ///Contains action methods to support GET, POST, PUT, and DELETE HTTP requests.
    [Route("api/users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            this._userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var isCreated  = await _userService.Create(user);
            if (isCreated == null)
                return Ok("Username Added:" + user.Username);
            else 
                return NotFound(new { message = isCreated});
        }

     
        [HttpGet]
        public async Task<ActionResult<List<UserViewModel>>> Get(){
			return await _userService.Get();
		}

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public async Task<ActionResult<UserViewModel>> GetAsync(string id)
        {
            var user = await _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return  user;
        }
        [Route("updateuser")]
        public async Task<ActionResult<User>> Update( User user)
        {
            User usr = await _userService.Update(user);

            if (usr == null)
            {
                return NotFound();
            }
            return usr;
        }
		
        [Route("restpassword")]
        [HttpPut()]
        public async Task<ActionResult<User>> RestPasswprd(PasswordResetRequest request)
        {
            User usr = await _userService.UpdatePassword(request.Id, request.Password);

            if (usr == null)
            {
                return NotFound();
            }
            return usr;
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.Remove(id);

            if (!user)
            {
                return NotFound();
            }

            return Ok(new { message = "Removed!" });
        }
    }
}

