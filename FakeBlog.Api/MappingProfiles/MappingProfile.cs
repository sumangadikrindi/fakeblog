using AutoMapper;
using FakeBlog.Api.DTOs;
using FakeBlog.Blogs.Api;
using FakeBlog.Users.Api;

namespace FakeBlog.Api.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetBlogResponse, BlogDTO>();
            CreateMap<GetUserDetailsResponse, UserDTO>();
            CreateMap<CreateBlogDTO, CreateBlogRequest>();
            CreateMap<CreateUserResponse, RegisterAuthorDTO>();
            CreateMap<RegisterAuthorDTO, CreateUserRequest>();
            CreateMap<CreateUserResponse, RegisterAuthorResponseDTO>();
        }
    }
}