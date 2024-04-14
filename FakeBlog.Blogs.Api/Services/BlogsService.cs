using AutoMapper;
using FakeBlog.Blogs.Api.Services.CommandHandlers;
using FakeBlog.Blogs.Api.Services.Commands;
using FakeBlog.Blogs.Api.Services.Queries;
using FakeBlog.Blogs.Api.Services.QueryHandlers;
using Grpc.Core;

namespace FakeBlog.Blogs.Api.Services;
public class BlogService : Api.BlogService.BlogServiceBase
{
    private readonly CreateBlogCommandHandler _createBlogCommandHandler;
    private readonly GetBlogQueryHandler _getBlogQueryHandler;
    private readonly IMapper _mapper;

    public BlogService(CreateBlogCommandHandler createBlogCommandHandler, GetBlogQueryHandler getBlogQueryHandler, IMapper mapper)
    {
        _createBlogCommandHandler = createBlogCommandHandler;
        _getBlogQueryHandler = getBlogQueryHandler;
        _mapper = mapper;
    }

    public override async Task<CreateBlogResponse> CreateBlog(CreateBlogRequest request, ServerCallContext context)
    {
        var blogId = await _createBlogCommandHandler.Handle(_mapper.Map<CreateBlogCommand>(request), context.CancellationToken);
        return new CreateBlogResponse() { Id = blogId.ToString() };
    }

    public override async Task<GetBlogResponse> GetBlog(GetBlogRequest request, ServerCallContext context)
    {
        var blogDTO = await _getBlogQueryHandler.Handle(_mapper.Map<GetBlogQuery>(request), context.CancellationToken);
        if (blogDTO == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Blog not found."));
        }
        return _mapper.Map<GetBlogResponse>(blogDTO);
    }
}