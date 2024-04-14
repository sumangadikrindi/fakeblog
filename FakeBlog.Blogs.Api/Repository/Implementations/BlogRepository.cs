using AutoMapper;
using FakeBlog.Blogs.Api.Data;
using FakeBlog.Blogs.Api.DTOs;
using FakeBlog.Blogs.Api.Models;
using FakeBlog.Blogs.Api.Repository.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FakeBlog.Blogs.Api.Repository.Implementations;

public class BlogRepository : IBlogRepository
{
    private readonly BlogsDbContext _blogsDbContext;
    private readonly IMapper _mapper;

    public BlogRepository(BlogsDbContext blogsDbContext, IMapper mapper)
    {
        _blogsDbContext = blogsDbContext;
        _mapper = mapper;
    }
    public async Task<Guid> CreateBlog(BlogDTO blogDTO)
    {
        var blog = _mapper.Map<Blog>(blogDTO);
        await _blogsDbContext.Blogs.AddAsync(blog);
        await _blogsDbContext.SaveChangesAsync();
        return blog.Id;
    }

    public async Task<BlogDTO?> GetBlog(Guid id)
    {
        var blog = await _blogsDbContext.Blogs.FindAsync(id);
        return _mapper.Map<BlogDTO>(blog);
    }
}
