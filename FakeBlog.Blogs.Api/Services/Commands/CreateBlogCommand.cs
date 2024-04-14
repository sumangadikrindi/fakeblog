using MediatR;

namespace FakeBlog.Blogs.Api.Services.Commands;

public class CreateBlogCommand : IRequest<Guid>
{
    public Guid AuthorId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
