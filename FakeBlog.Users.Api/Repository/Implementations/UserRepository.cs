using AutoMapper;
using FakeBlog.Users.Api.Data;
using FakeBlog.Users.Api.Models;
using FakeBlog.Users.Api.Repository.Contracts;
using FakeBlog.Users.Api.DTOs;

namespace FakeBlog.Users.Api.Repository.Implementations;
public class UserRepository : IUserRepository
{
    private readonly UsersDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserRepository(UsersDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<Guid> CreateUser(UserDTO userDTO)
    {
        var user = _mapper.Map<User>(userDTO);
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user.Id;
    }

    public async Task<UserDTO?> GetUser(Guid id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        return _mapper.Map<UserDTO>(user);
    }
}