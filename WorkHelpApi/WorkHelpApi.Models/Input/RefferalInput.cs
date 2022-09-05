namespace WorkHelpApi.Models.Input;

public class RefferalInput
{
	public RefferalInput()
	{

	}

	public int Id { get; set; }
	public string? RefferalUserIdTelegram { get; set; }
	public string? RefferalNicknameTelegram { get; set; }
	public string? RefferUserIdTelegram { get; set; }
	public string? RefferNicknameTelegram { get; set; }
	public int Earned { get; set; }
	public int RefferalsCount { get; set; }
}


