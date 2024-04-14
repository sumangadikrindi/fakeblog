using AutoMapper;
using FakeBlog.Users.Api.Models;
using FakeBlog.Users.Api.DTOs;
using FakeBlog.Users.Api.Services.Queries;
using FakeBlog.Users.Api.Services.Commands;

namespace FakeBlog.Users.Api.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDTO>();
        CreateMap<UserDTO, User>();
        CreateMap<CreateUserRequest, CreateUserCommand>();
        CreateMap<GetUserDetailsRequest, GetUserDetailsQuery>();
        CreateMap<UserDTO, GetUserDetailsResponse>();
        CreateMap<CreateUserCommand, UserDTO>();
    }
}
