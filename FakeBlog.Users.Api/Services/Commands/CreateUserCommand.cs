using MediatR;

namespace FakeBlog.Users.Api.Services.Commands;
public class CreateUserCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
}
