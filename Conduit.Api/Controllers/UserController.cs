using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Api.Dto.User;
using Conduit.Core.Models;
using Conduit.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {   
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Register([FromBody] UserPostDto userDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = _mapper.Map<User>(userDto);
            await _userService.CreateUser(user);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }
    }
}