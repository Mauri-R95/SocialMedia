using AutoMapper;
using SocialMedia.Api.Application.Model;
using SocialMedia.Core.Entities;

namespace SocialMedia.Api.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Post, PostDto>();
            CreateMap<PostDto, Post>();
            CreateMap<Security, SecurityDto>().ReverseMap();
            //CreateMap<SecurityDto, Security >();
        }
    }
}
