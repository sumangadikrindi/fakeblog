using System.Net;
using AutoMapper;
using FakeBlog.Api.DTOs;
using FakeBlog.Users.Api;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using static FakeBlog.Users.Api.UserService;

namespace FakeBlog.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    private readonly UserServiceClient _userServiceClient;
    private readonly IMapper _mapper;

    public AuthorController(UserServiceClient userServiceClient, IMapper mapper)
    {
        _userServiceClient = userServiceClient;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(RegisterAuthorResponseDTO), (int)HttpStatusCode.Created)]
    [Route("RegisterAuthor")]
    public async Task<IActionResult> RegisterAuthor(RegisterAuthorDTO createBlogDTO)
    {
        var createUserResponse = await _userServiceClient.CreateUserAsync(_mapper.Map<CreateUserRequest>(createBlogDTO));
        var registerAuthorResponseDTO = _mapper.Map<RegisterAuthorResponseDTO>(createUserResponse);
        var uri = Url.Action(null, null, registerAuthorResponseDTO.Id, Request.Scheme);
        return Created(uri, registerAuthorResponseDTO);
    }

    [HttpGet]
    [Route("{authorId}")]
    [ProducesResponseType(typeof(UserDTO), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAuthorDetails(Guid authorId)
    {
        try
        {

            var userDetails = await _userServiceClient.GetUserDetailsAsync(new GetUserDetailsRequest { Id = authorId.ToString() });
            var userDTO = _mapper.Map<UserDTO>(userDetails);

            return Ok(userDTO);
        }
        catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return NotFound();
        }
    }
}
