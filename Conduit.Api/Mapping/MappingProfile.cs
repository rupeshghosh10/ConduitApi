using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Api.Dto.Article;
using Conduit.Api.Dto.Comment;
using Conduit.Api.Dto.Profile;
using Conduit.Api.Dto.User;
using Conduit.Core.Models;

namespace Conduit.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserPostDto>().ReverseMap();
            CreateMap<User, UserResponseDto>().ReverseMap();
            CreateMap<User, UserPutDto>().ReverseMap();

            CreateMap<User, ProfileDto>().ReverseMap();

            CreateMap<string, Tag>()
                .ConstructUsing(x => new Tag { Text = x })
                .ReverseMap()
                .ConstructUsing(x => x.Text);

            CreateMap<Article, ArticleDto>().ReverseMap();
            CreateMap<Article, ArticlePostDto>().ReverseMap();
            CreateMap<Article, ArticlePutDto>().ReverseMap();

            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Comment, CommentPostDto>().ReverseMap();
        }
    }
}