using FakeBlog.Users.Api.DTOs;

namespace FakeBlog.Users.Api.Repository.Contracts;
public interface IUserRepository
{
    Task<Guid> CreateUser(UserDTO userDTO);
    Task<UserDTO?> GetUser(Guid id);
}