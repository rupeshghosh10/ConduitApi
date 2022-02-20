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
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetArticles(
            [FromQuery] string tag = "",
            [FromQuery] string author = "",
            [FromQuery] int limit = 10,
            [FromQuery] int offset = 0)
        {
            var articlesInDb = (await _articleService.GetArticles(tag, author, limit, offset)).ToList();
            var articlesDto = articlesInDb.Select(x => _mapper.Map<ArticleDto>(x)).ToList();

            try
            {
                for (int i = 0; i < articlesInDb.Count; i++)
                {
                    articlesDto[i].Author.IsFollowing = _userService.IsFollowing(_tokenManager.GetUserId(), articlesInDb[i].Author);
                }
            }
            catch { }

            return Ok(articlesDto);
        }

        [HttpGet]
        [Route("{slug}")]
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
            }
            catch { }

            return Ok(articleDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ArticleDto>> PostArticle([FromBody] ArticlePostDto articlePostDto)
        {
            if (!ModelState.IsValid)
            {
                return Unauthorized();
            }

            var article = _mapper.Map<Article>(articlePostDto);
            var articleInDb = await _articleService.CreateArticle(article, _tokenManager.GetUserId());

            return Ok(_mapper.Map<ArticleDto>(articleInDb));
        }

        [HttpPut]
        [Authorize]
        [Route("{slug}")]
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

        [HttpGet]
        [Route("{slug}/comments")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments([FromRoute] string slug)
        {
            var articleInDb = await _articleService.GetArticle(slug);
            if (articleInDb == null)
            {
                return NotFound();
            }

            var commentsInDb = await _commentService.GetComments(slug);
            var commentsDto = commentsInDb.Select(x => _mapper.Map<CommentDto>(x)).ToList();

            try
            {
                for (int i = 0; i < commentsDto.Count; i++)
                {
                    commentsDto[i].Author.IsFollowing = _userService.IsFollowing(_tokenManager.GetUserId(), articleInDb.Author);
                }
            }
            catch { }

            return Ok(commentsDto);
        }

        [HttpPost]
        [Authorize]
        [Route("{slug}/comments")]
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
    }
}