using FakeBlog.Users.Api.DTOs;
using MediatR;

namespace FakeBlog.Users.Api.Services.Queries;
public class GetUserDetailsQuery : IRequest<UserDTO?>
{
    public Guid Id { get; set; }
}
