using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Api.Dto.Profile;
using Conduit.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Controllers
{
    [ApiController]
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ITokenManager _tokenManager;

        public ProfileController(IMapper mapper, IUserService userService, ITokenManager tokenManager)
        {
            _mapper = mapper;
            _userService = userService;
            _tokenManager = tokenManager;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<ProfileDto>> GetProfile([FromRoute] string username)
        {
            var userInDb = await _userService.GetByUsername(username);
            if (userInDb == null)
            {
                return NotFound();
            }

            var profileDto = _mapper.Map<ProfileDto>(userInDb);

            return Ok(profileDto);
        }

        [HttpPost]
        [Authorize]
        [Route("{username}/follow")]
        public async Task<ActionResult<ProfileDto>> FollowProfile([FromRoute] string username)
        {
            var followedUser = await _userService.GetByUsername(username);
            if (followedUser == null)
            {
                return NotFound();
            }

            var curentUser = await _userService.GetByEmailIncludeFollowing(_tokenManager.GetUserEmail());

            await _userService.AddFollower(curentUser, followedUser);
            
            var profileDto = _mapper.Map<ProfileDto>(followedUser);
            profileDto.Following = true;

            return Ok(profileDto);
        }
    }
}