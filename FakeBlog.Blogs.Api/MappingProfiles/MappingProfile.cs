using AutoMapper;
using FakeBlog.Blogs.Api.DTOs;
using FakeBlog.Blogs.Api.Models;
using FakeBlog.Blogs.Api.Services.Commands;
using FakeBlog.Blogs.Api.Services.Queries;

namespace FakeBlog.Blogs.Api.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BlogDTO, Blog>();
        CreateMap<Blog, BlogDTO>();
        CreateMap<CreateBlogRequest, CreateBlogCommand>();
        CreateMap<CreateBlogCommand, BlogDTO>();
        CreateMap<GetBlogRequest, GetBlogQuery>();
        CreateMap<BlogDTO, GetBlogResponse>();
    }
}
