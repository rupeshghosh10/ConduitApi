using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Api.Dto.User;
using Conduit.Core.Models;

namespace Conduit.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserPostDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}