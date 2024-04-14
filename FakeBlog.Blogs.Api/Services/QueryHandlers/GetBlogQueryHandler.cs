using AutoMapper;
using FakeBlog.Blogs.Api.DTOs;
using FakeBlog.Blogs.Api.Models;
using FakeBlog.Blogs.Api.Repository.Contracts;
using FakeBlog.Blogs.Api.Services.Queries;
using MediatR;

namespace FakeBlog.Blogs.Api.Services.QueryHandlers;
public class GetBlogQueryHandler : IRequestHandler<GetBlogQuery, BlogDTO?>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;

    public GetBlogQueryHandler(IBlogRepository blogRepository, IMapper mapper)
    {
        _blogRepository = blogRepository;
        _mapper = mapper;
    }
    public async Task<BlogDTO?> Handle(GetBlogQuery request, CancellationToken cancellationToken)
    {
        return await _blogRepository.GetBlog(request.Id);
    }
}