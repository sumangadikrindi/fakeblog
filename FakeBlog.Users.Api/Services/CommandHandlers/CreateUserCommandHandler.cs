using AutoMapper;
using FakeBlog.Users.Api.Repository.Contracts;
using FakeBlog.Users.Api.DTOs;
using FakeBlog.Users.Api.Services.Commands;
using MediatR;

namespace FakeBlog.Users.Api.Services.CommandHandlers;
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        this._userRepository = userRepository;
        _mapper = new MapperConfiguration(config =>
        {
            config.CreateMap<CreateUserCommand, UserDTO>();
        }).CreateMapper();
    }
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return await _userRepository.CreateUser(_mapper.Map<UserDTO>(request));
    }
}
