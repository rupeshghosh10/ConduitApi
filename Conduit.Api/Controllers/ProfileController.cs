using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Api.Dto.Profile;
using Conduit.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProfileDto>> GetProfile([FromRoute] string username)
        {
            var userInDb = await _userService.GetByUsernameWithFollowers(username);
            if (userInDb == null)
            {
                return NotFound();
            }

            var profileDto = _mapper.Map<ProfileDto>(userInDb);

            try
            {
                profileDto.IsFollowing = _userService.IsFollowing(_tokenManager.GetUserId(), userInDb);
            }
            catch {}

            return Ok(profileDto);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Route("{username}/follow")]
        public async Task<ActionResult<ProfileDto>> FollowProfile([FromRoute] string username)
        {
            var followedUser = await _userService.GetByUsernameWithFollowers(username);
            if (followedUser == null)
            {
                return NotFound();
            }

            var currentUserId = _tokenManager.GetUserId();

            if (_userService.IsFollowing(currentUserId, followedUser) || currentUserId == followedUser.UserId)
            {
                return Conflict();
            }

            await _userService.AddFollower(currentUserId, followedUser);

            var profileDto = _mapper.Map<ProfileDto>(followedUser);
            profileDto.IsFollowing = true;

            return Ok(profileDto);
        }

        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Route("{username}/follow")]
        public async Task<ActionResult> UnfollowProfile([FromRoute] string username)
        {
            var followedUser = await _userService.GetByUsernameWithFollowers(username);
            if (followedUser == null)
            {
                return NotFound();
            }

            if (!_userService.IsFollowing(_tokenManager.GetUserId(), followedUser))
            {
                return Conflict();
            }

            await _userService.DeleteFollower(_tokenManager.GetUserId(), followedUser);

            return NoContent();
        }
    }
}