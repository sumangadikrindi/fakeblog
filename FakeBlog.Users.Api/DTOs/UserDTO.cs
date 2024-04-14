namespace FakeBlog.Users.Api.DTOs;

public class UserDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Lastname { get; set; }= string.Empty;
}