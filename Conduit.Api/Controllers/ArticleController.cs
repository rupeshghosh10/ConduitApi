using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Api.Dto.Article;
using Conduit.Api.Dto.Comment;
using Conduit.Core.Models;
using Conduit.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Controllers
{
    [ApiController]
    [Route("api/article")]
    public class ArticleController : ControllerBase
    {
        private readonly ITokenManager _tokenManager;
        private readonly IMapper _mapper;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;

        public ArticleController(
            ITokenManager tokenManager,
            IMapper mapper,
            IArticleService articleService,
            ICommentService commentService,
            IUserService userService)
        {
            _tokenManager = tokenManager;
            _mapper = mapper;
            _articleService = articleService;
            _commentService = commentService;
            _userService = userService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetArticles(
            [FromQuery] string tag = "",
            [FromQuery] string author = "",
            [FromQuery] string favorited = "",
            [FromQuery] int limit = 5,
            [FromQuery] int offset = 0)
        {
            var articlesInDb = (await _articleService.GetArticles(tag, author, favorited, limit, offset)).ToList();
            var articlesDto = articlesInDb.Select(x => _mapper.Map<ArticleDto>(x)).ToList();

            try
            {
                for (int i = 0; i < articlesInDb.Count; i++)
                {
                    articlesDto[i].Author.IsFollowing = _userService.IsFollowing(_tokenManager.GetUserId(), articlesInDb[i].Author);
                    articlesDto[i].Favorited = _userService.IsFavourite(_tokenManager.GetUserId(), articlesInDb[i]);
                }
            }
            catch { }

            return Ok(articlesDto);
        }

        [HttpGet]
        [Authorize]
        [Route("feed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetArticlesFeed(
            [FromQuery] int limit = 5,
            [FromQuery] int offset = 0)
        {
            var articlesInDb = (await _articleService.GetArticlesFeed(limit, offset, _tokenManager.GetUserId())).ToList();
            var articlesDto = articlesInDb.Select(x => _mapper.Map<ArticleDto>(x)).ToList();

            for (int i = 0; i < articlesInDb.Count; i++)
            {
                articlesDto[i].Author.IsFollowing = _userService.IsFollowing(_tokenManager.GetUserId(), articlesInDb[i].Author);
                articlesDto[i].Favorited = _userService.IsFavourite(_tokenManager.GetUserId(), articlesInDb[i]);
            }

            return Ok(articlesDto);
        }

        [HttpGet]
        [Route("{slug}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ArticleDto>> GetArticle([FromRoute] string slug)
        {
            var articleInDb = await _articleService.GetArticle(slug);
            if (articleInDb == null)
            {
                return NotFound();
            }

            var articleDto = _mapper.Map<ArticleDto>(articleInDb);
            try
            {
                articleDto.Author.IsFollowing = _userService.IsFollowing(_tokenManager.GetUserId(), articleInDb.Author);
                articleDto.Favorited = _userService.IsFavourite(_tokenManager.GetUserId(), articleInDb);
            }
            catch { }

            return Ok(articleDto);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ArticleDto>> PostArticle([FromBody] ArticlePostDto articlePostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var article = _mapper.Map<Article>(articlePostDto);
            var articleInDb = await _articleService.CreateArticle(article, _tokenManager.GetUserId());

            return Ok(_mapper.Map<ArticleDto>(articleInDb));
        }

        [HttpPut]
        [Authorize]
        [Route("{slug}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ArticleDto>> PutArticle([FromBody] ArticlePutDto articlePutDto, [FromRoute] string slug)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var articleInDb = await _articleService.GetArticle(slug);
            if (articleInDb == null)
            {
                return NotFound();
            }

            if (_tokenManager.GetUserId() != articleInDb.AuthorId)
            {
                return Forbid();
            }

            var newArticle = await _articleService.UpdateArticle(articleInDb, _mapper.Map<Article>(articlePutDto));

            return Ok(_mapper.Map<ArticleDto>(newArticle));
        }

        [HttpDelete]
        [Authorize]
        [Route("{slug}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteArticle([FromRoute] string slug)
        {
            var articleInDb = await _articleService.GetArticle(slug);
            if (articleInDb == null)
            {
                return NotFound();
            }

            if (articleInDb.AuthorId != _tokenManager.GetUserId())
            {
                return Forbid();
            }

            await _articleService.DeleteArticle(articleInDb);

            return NoContent();
        }

        [HttpGet]
        [Route("{slug}/comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ApiExplorerSettings(GroupName = "Comments")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments([FromRoute] string slug)
        {
            var articleInDb = await _articleService.GetArticle(slug);
            if (articleInDb == null)
            {
                return NotFound();
            }

            var commentsInDb = (await _commentService.GetComments(slug)).ToList();
            var commentsDto = commentsInDb.Select(x => _mapper.Map<CommentDto>(x)).ToList();

            try
            {
                for (int i = 0; i < commentsDto.Count; i++)
                {
                    commentsDto[i].Author.IsFollowing = _userService.IsFollowing(_tokenManager.GetUserId(), commentsInDb[i].Author);
                }
            }
            catch { }

            return Ok(commentsDto);
        }

        [HttpPost]
        [Authorize]
        [Route("{slug}/comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ApiExplorerSettings(GroupName = "Comments")]
        public async Task<ActionResult<CommentDto>> PostComment([FromBody] CommentPostDto commentPostDto, [FromRoute] string slug)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var articleInDb = await _articleService.GetArticle(slug);
            if (articleInDb == null)
            {
                return NotFound();
            }

            var comment = await _commentService.AddComment(
                _mapper.Map<Comment>(commentPostDto),
                articleInDb.ArticleId,
                _tokenManager.GetUserId());

            var commentDto = _mapper.Map<CommentDto>(comment);
            commentDto.Author.IsFollowing = _userService.IsFollowing(_tokenManager.GetUserId(), articleInDb.Author);

            return Ok(commentDto);
        }

        [HttpDelete]
        [Authorize]
        [Route("{slug}/comments/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ApiExplorerSettings(GroupName = "Comments")]
        public async Task<ActionResult> DeleteComment([FromRoute] string slug, [FromRoute] int id)
        {
            var articleInDb = await _articleService.GetArticle(slug);
            if (articleInDb == null)
            {
                return NotFound();
            }

            var commentInDb = await _commentService.GetComment(id);
            if (commentInDb == null)
            {
                return NotFound();
            }

            if (commentInDb.AuthorId != _tokenManager.GetUserId())
            {
                return Forbid();
            }

            await _commentService.DeleteComment(commentInDb);

            return NoContent();
        }

        [HttpPost]
        [Authorize]
        [Route("{slug}/favorite")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ApiExplorerSettings(GroupName = "Favorites")]
        public async Task<ActionResult> FavoriteArticle([FromRoute] string slug)
        {
            var articleInDb = await _articleService.GetArticle(slug);
            if (articleInDb == null)
            {
                return NotFound();
            }

            if (_articleService.IsFavourite(articleInDb, _tokenManager.GetUserId()))
            {
                return Conflict();
            }

            await _articleService.FavoriteArticle(articleInDb, _tokenManager.GetUserId());

            return NoContent();
        }

        [HttpDelete]
        [Authorize]
        [Route("{slug}/favorite")]
        [ApiExplorerSettings(GroupName = "Favorites")]
        public async Task<ActionResult> DeleteFavoriteArticle([FromRoute] string slug)
        {
            var articleInDb = await _articleService.GetArticle(slug);
            if (articleInDb == null)
            {
                return NotFound();
            }

            if (!_articleService.IsFavourite(articleInDb, _tokenManager.GetUserId()))
            {
                return Conflict();
            }

            await _articleService.UnFavoriteArticle(articleInDb, _tokenManager.GetUserId());

            return NoContent();
        }
    }
}