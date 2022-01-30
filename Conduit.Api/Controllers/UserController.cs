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
        private readonly IPasswordManager _passwordManager;

        public UserController(IUserService userService, IMapper mapper, IPasswordManager passwordManager)
        {
            _userService = userService;
            _mapper = mapper;
            _passwordManager = passwordManager;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Register([FromBody] UserPostDto userDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = _mapper.Map<User>(userDto);

            user.Salt = _passwordManager.GenerateSalt();
            user.Password = _passwordManager.HashPassword(userDto.Password, user.Salt);
            
            await _userService.CreateUser(user);

            return Ok(user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<User>> Login([FromBody] UserLoginDto userDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            
            var userInDb = await _userService.GetByEmail(userDto.Email);
            if (userInDb == null) return NotFound();
            
            if (_passwordManager.VerifyPassword(userDto.Password, userInDb.Password, userInDb.Salt))
            {
                return Ok(userInDb);
            }
            else 
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }
    }
}