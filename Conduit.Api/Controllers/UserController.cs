using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Api.Dto.User;
using Conduit.Core.Models;
using Conduit.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IPasswordManager _passwordManager;
        private readonly ITokenManager _tokenManager;

        public UserController(
            IUserService userService,
            IMapper mapper,
            IPasswordManager passwordManager,
            ITokenManager tokenManager)
        {
            _userService = userService;
            _mapper = mapper;
            _passwordManager = passwordManager;
            _tokenManager = tokenManager;
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> Register([FromBody] UserPostDto userPostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (await _userService.IsUniqueEmail(userPostDto.Email)
                || await _userService.IsUniqueUsername(userPostDto.Username))
            {
                return Conflict();
            }

            var user = _mapper.Map<User>(userPostDto);
            user.Password = _passwordManager.GeneratePassword(userPostDto.Password);

            await _userService.CreateUser(user);

            var userDto = _mapper.Map<UserResponseDto>(user);
            userDto.Token = _tokenManager.GenerateToken(user.Email);

            return Ok(userDto);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserResponseDto>> Login([FromBody] UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userInDb = await _userService.GetByEmail(userLoginDto.Email);
            if (userInDb == null)
            {
                return NotFound();
            }

            if (_passwordManager.VerifyPassword(userLoginDto.Password, userInDb.Password))
            {
                var userDto = _mapper.Map<UserResponseDto>(userInDb);
                userDto.Token = _tokenManager.GenerateToken(userLoginDto.Email);

                return Ok(userDto);
            }

            return Unauthorized();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUser()
        {
            var user = await _userService.GetByEmail(_tokenManager.GetUserEmail());
            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> UpdateUser([FromBody] UserPutDto userPutDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userInDb = await _userService.GetByEmail(_tokenManager.GetUserEmail());
            if (userInDb == null)
            {
                return NotFound();
            }

            if (await _userService.IsUniqueUsername(userPutDto.Username, userInDb.Username))
            {
                return Conflict();
            }

            await _userService.UpdateUser(userInDb, _mapper.Map<User>(userPutDto));

            return NoContent();
        }

        [HttpPost]
        [Authorize]
        [Route("resetpassword")]
        public async Task<ActionResult> ChangePassword([FromBody] UserResetPasswordDto userResetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userInDb = await _userService.GetByEmail(_tokenManager.GetUserEmail());
            if (userInDb == null)
            {
                return NotFound();
            }

            if (_passwordManager.VerifyPassword(userResetPasswordDto.OldPassword, userInDb.Password))
            {
                string password = _passwordManager.GeneratePassword(userResetPasswordDto.NewPassword);
                await _userService.UpdatePassword(userInDb, password);
                return NoContent();
            }

            return Forbid();
        }
    }
}