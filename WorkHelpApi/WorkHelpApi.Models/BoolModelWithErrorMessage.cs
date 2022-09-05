namespace WorkHelpApi.Models;

public class BoolModelWithErrorMessage
{
	public BoolModelWithErrorMessage()
	{

	}

	public bool IsError { get; set; }
	public string? ErrorMessage { get; set; }
}
