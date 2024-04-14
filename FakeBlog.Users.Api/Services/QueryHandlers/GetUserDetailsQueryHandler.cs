using FakeBlog.Users.Api.Repository.Contracts;
using FakeBlog.Users.Api.DTOs;
using FakeBlog.Users.Api.Services.Queries;
using MediatR;

namespace FakeBlog.Users.Api.Services.QueryHandlers;
public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDTO?>
{
    private readonly IUserRepository _userRepository;
    public GetUserDetailsQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<UserDTO?> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetUser(request.Id);
    }
}
