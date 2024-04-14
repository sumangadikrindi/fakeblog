using AutoMapper;
using FakeBlog.Users.Api.Services.Queries;
using FakeBlog.Users.Api.Services.QueryHandlers;
using FakeBlog.Users.Api.Services.CommandHandlers;
using Grpc.Core;
using static FakeBlog.Users.Api.UserService;
using System.Security.Permissions;

namespace FakeBlog.Users.Api.Services
{
    public class UserService : UserServiceBase
    {
        private readonly CreateUserCommandHandler _createUserCommandHandler;
        private readonly GetUserDetailsQueryHandler _getUserDetailsQueryHandler;
        private readonly IMapper _mapper;

        public UserService(CreateUserCommandHandler createUserCommandHandler, GetUserDetailsQueryHandler getUserDetailsQueryHandler, IMapper mapper)
        {
            _createUserCommandHandler = createUserCommandHandler;
            _getUserDetailsQueryHandler = getUserDetailsQueryHandler;
            _mapper = mapper;
        }

        public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            var userId = await _createUserCommandHandler.Handle(_mapper.Map<Commands.CreateUserCommand>(request), context.CancellationToken);
            return new CreateUserResponse(){Id = userId.ToString()};
        }

        public override async Task<GetUserDetailsResponse> GetUserDetails(GetUserDetailsRequest request, ServerCallContext context)
        {
            var userDTO = await _getUserDetailsQueryHandler.Handle(_mapper.Map<GetUserDetailsQuery>(request), context.CancellationToken);
            if(userDTO == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found."));
            }
            return _mapper.Map<GetUserDetailsResponse>(userDTO);
        }
    }
}