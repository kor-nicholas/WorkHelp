using Deployf.Botf;
using Newtonsoft.Json;

using WorkHelpBot.Models.Output;

namespace WorkHelpBot.Controllers;

public class PromocodeConfigurationController : BotController
{
	readonly IConfiguration _configuration;
	readonly ILogger<PromocodeConfigurationController> _logger;

	public PromocodeConfigurationController(IConfiguration configuration, ILogger<PromocodeConfigurationController> logger)
	{
		_configuration = configuration;
		_logger = logger;
	}

	[Authorize("admin")]
	[Action("/add-promocode")]
	public async Task AddPromocode()
	{
		PushL("Введите новый промокод: ");
		await Send();
		var name = await AwaitText();

		PushL("Введите дату окончания промокода(yyyy-mm-dd): ");
		await Send();
		var endDate = await AwaitText();

		PushL("Введите сумму бонуса: ");
		await Send();
		var bonus = await AwaitText();

		using(var client = new HttpClient())
		{
			var endPoint = new Uri($"{_configuration["apiUrl"]}/promocode/add/{name}/{endDate}/{bonus}");
			var json = client.PostAsync(endPoint, new StringContent("")).Result.Content.ReadAsStringAsync().Result;

			_logger.LogInformation($"Result(json): {json}");
		}

		PushL("Промокод добавлен");
		await Send();
	}

	[Authorize("admin")]
	[Action("/delete-promocode")]
	public async Task DeletePromocode()
	{
		PushL("Введите название промокода для удаления: ");
		await Send();
		var promo = await AwaitText();

		using(var client = new HttpClient())
		{
			var endPoint = new Uri($"{_configuration["apiUrl"]}/promocode/delete/{promo}");
			var json = client.DeleteAsync(endPoint).Result.Content.ReadAsStringAsync().Result;

			_logger.LogInformation($"Result(json): {json}");
		}

		PushL("Промокод удален");
		await Send();
	}

	[Authorize("admin")]
	[Action("/promocodes")]
	public async Task GetPromocodes()
	{
		List<PromocodeOutput> promocodes = null;

		using(var client = new HttpClient())
		{
			var endPoint = new Uri($"{_configuration["apiUrl"]}/promocode/promocodes");
			var json = client.GetAsync(endPoint).Result.Content.ReadAsStringAsync().Result;
			promocodes = JsonConvert.DeserializeObject<List<PromocodeOutput>>(json);

			_logger.LogInformation($"Result(json): {json}");
		}

		PushL("Все промокоды: \n");
		foreach(var promocode in promocodes)
		{
			PushL($"Promo: {promocode.Name} | EndDate: {promocode.EndDate} | Bonus: {promocode.Bonus}\n ActivatedUserId: {promocode.ActivatedUserId}");
		}
		await Send();
	}
}




