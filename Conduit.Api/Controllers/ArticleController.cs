using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Api.Dto.Article;
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
        private readonly IArticleManager _articleManager;

        public ArticleController(ITokenManager tokenManager, IMapper mapper, IArticleManager articleManager)
        {
            _tokenManager = tokenManager;
            _mapper = mapper;
            _articleManager = articleManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ArticleDto>> CreateArticle([FromBody] ArticlePostDto articlePostDto)
        {
            if (!ModelState.IsValid) 
            {
                return Unauthorized();
            }

            var article = _mapper.Map<Article>(articlePostDto);
            var articleInDb = await _articleManager.CreateArticle(article, _tokenManager.GetUserId());     

            return Ok(_mapper.Map<ArticleDto>(articleInDb));
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetArticles(
            [FromQuery] string tag = "",
            [FromQuery] string author = "",
            [FromQuery] int limit = 10,
            [FromQuery] int offset = 0)
        {
            var articles = await _articleManager.GetArticles(tag, author, limit, offset);
            var articlesDto = articles.Select(x => _mapper.Map<ArticleFeedDto>(x));

            return Ok(articlesDto);
        }
    }
}