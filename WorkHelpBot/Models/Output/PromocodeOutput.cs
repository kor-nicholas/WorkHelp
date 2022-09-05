namespace WorkHelpBot.Models.Output;

public class PromocodeOutput
{
	public PromocodeOutput()
	{

	}

	public int Id { get; set; }
	public string Name { get; set; }
	public DateTime EndDate { get; set; }
	public double Bonus { get; set; }
	public string ActivatedUserId { get; set; }
}


