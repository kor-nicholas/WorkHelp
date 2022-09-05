namespace WorkHelpBot.Models.Input;

public class WorkInput
{
	public WorkInput()
	{

	}

	public int? Id { get; set; }
	public string? Category { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
	public int Price { get; set; }
	public string? Link { get; set; }
}
