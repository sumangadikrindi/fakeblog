using FakeBlog.Blogs.Api.DTOs;

namespace FakeBlog.Blogs.Api.Repository.Contracts;
public interface IBlogRepository
{
    Task<Guid> CreateBlog(BlogDTO blogDTO);
    Task<BlogDTO?> GetBlog(Guid id);
}