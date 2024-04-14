using AutoMapper;
using FakeBlog.Blogs.Api.DTOs;
using FakeBlog.Blogs.Api.Repository.Contracts;
using FakeBlog.Blogs.Api.Services.Commands;
using MediatR;

namespace FakeBlog.Blogs.Api.Services.CommandHandlers;

public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommand, Guid>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;

    public CreateBlogCommandHandler(IBlogRepository blogRepository, IMapper mapper)
    {
        _blogRepository = blogRepository;
        _mapper = mapper;
    }
    public async Task<Guid> Handle(CreateBlogCommand command, CancellationToken cancellationToken)
    {
        return await _blogRepository.CreateBlog(_mapper.Map<BlogDTO>(command));
    }
}
