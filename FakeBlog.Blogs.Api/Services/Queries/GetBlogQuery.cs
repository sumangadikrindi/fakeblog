using FakeBlog.Blogs.Api.DTOs;
using MediatR;

namespace FakeBlog.Blogs.Api.Services.Queries;

public class GetBlogQuery : IRequest<BlogDTO>
{ 
    public Guid Id { get; set; }
}
