using System;
using CRUDAPI.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SharedModelNamespace.Shared.ViewModels;

namespace CRUDAPI.Controllers
{ 
//class to perform CRUD operations
///Contains action methods to support GET, POST, PUT, and DELETE HTTP requests.

    [Authorize]
    [Route("api/user-control")]
    [ApiController]
    public class UserControlAPIController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserControlAPIController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet]
        public ActionResult<List<UserViewModel>> Get() =>
            _userService.Get();

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public ActionResult<UserViewModel> Get(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }


    }
}
