namespace WorkHelpApi.Models.Entities;

public class PromocodeEntity
{
	public PromocodeEntity()
	{

	}

	public int Id { get; set; }
	public string Name { get; set; }
	public DateTime EndDate { get; set; }
	public decimal Bonus { get; set; }
	public string ActivatedUserId { get; set; }
}



