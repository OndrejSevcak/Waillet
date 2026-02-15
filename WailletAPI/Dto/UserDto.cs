namespace WailletAPI.Dto;

public class UserDto
{
    public long UserKey { get; set; }
    public required string UserName { get; set; }
    public DateTime CreatedAt { get; set; }
}