using System.Net;
using System.Text.Json;
using AutoMapper;
using FakeBlog.Api.DTOs;
using FakeBlog.Blogs.Api;
using FakeBlog.Users.Api;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using static FakeBlog.Blogs.Api.BlogService;
using static FakeBlog.Users.Api.UserService;

[ApiController]
[Route("[controller]")]
public class BlogController : ControllerBase
{
    private readonly UserServiceClient _userServiceClient;
    private readonly BlogServiceClient _blogServiceClient;
    private readonly IMapper _mapper;
    private readonly ILogger<BlogController> _logger;

    public BlogController(UserServiceClient userServiceClient, BlogServiceClient blogServiceClient, IMapper mapper, ILogger<BlogController> logger)
    {
        _userServiceClient = userServiceClient;
        _blogServiceClient = blogServiceClient;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    [Route("{blogId}")]
    [ProducesResponseType(typeof(BlogDTO), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetIncludingUserDetails(Guid blogId, [FromHeader(Name = "IncludeUserDetails")] bool includeUserDetails)
    {
        try
        {
            var getBlogResponse = await _blogServiceClient.GetBlogAsync(new GetBlogRequest { Id = blogId.ToString() });
            var blogDTO = _mapper.Map<BlogDTO>(getBlogResponse);
            if (includeUserDetails && blogDTO.AuthorId != Guid.Empty)
            {
                var userDetails = await _userServiceClient.GetUserDetailsAsync(new GetUserDetailsRequest { Id = blogDTO.AuthorId.ToString() });
                blogDTO.Author = _mapper.Map<UserDTO>(userDetails);
            }
            return Ok(blogDTO);
        }
        catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateBlogResponse), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreatePost(CreateBlogDTO createBlogDTO)
    {
        var createBlogRequest = _mapper.Map<CreateBlogRequest>(createBlogDTO);
        createBlogRequest.AuthorId = Guid.NewGuid().ToString(); //ToDo: Authorize and get author id

        var createBlogResponse = await _blogServiceClient.CreateBlogAsync(createBlogRequest);
        var uri = Url.Action(null, null, createBlogResponse.Id, Request.Scheme);
        return Created(uri, createBlogResponse);
    }
}