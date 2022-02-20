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

        public ArticleController(
            ITokenManager tokenManager,
            IMapper mapper,
            IArticleService articleService,
            ICommentService commentService)
        {
            _tokenManager = tokenManager;
            _mapper = mapper;
            _articleService = articleService;
            _commentService = commentService;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetArticles(
            [FromQuery] string tag = "",
            [FromQuery] string author = "",
            [FromQuery] int limit = 10,
            [FromQuery] int offset = 0)
        {
            var articles = await _articleService.GetArticles(tag, author, limit, offset);
            var articlesDto = articles.Select(x => _mapper.Map<ArticleDto>(x));

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

            return Ok(_mapper.Map<ArticleDto>(articleInDb));
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

            var comments = await _commentService.GetComments(slug);
            var commentsDto = comments.Select(x => _mapper.Map<CommentDto>(x));

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

            return Ok(_mapper.Map<CommentDto>(comment));
        }
    }
}