namespace WorkHelpApi.Models.Output;

public class UserOutput
{
	public UserOutput()
	{

	}

	public int Id { get; set; }
	public string? Name { get; set; }
	public string? Surname { get; set; }
	public string? Phone { get; set; }
	public string? Email { get; set; }
	public string? UserIdTelegram { get; set; }
	public string? NicknameTelegram { get; set; }
	public string? Pass { get; set; }
	public string? Login { get; set; }
	public string? Role { get; set; }
	public int Balance { get; set; }
	public string? Salt { get; set; }
	public string? RefferUserIdTelegram { get; set; }
	public string? RefferNicknameTelegram { get; set; }
	public int CompletedTaskCount { get; set; }
	public int RefferalsCount { get; set; }
}
