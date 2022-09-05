using Deployf.Botf;
using Newtonsoft.Json;

using WorkHelpBot.Models.Output;

class PromocodeController : BotController
{
	readonly IConfiguration _configuration;
	readonly ILogger _logger;

	public PromocodeController(IConfiguration configuration, ILogger<PromocodeController> logger)
	{
		_configuration = configuration;
		_logger = logger;
	}

	[Action("🎟промокод🎟")]
	public async Task Promocode()
	{
		PushL("Введите промокод:");
		await Send();

		string promocodeFromUser = await AwaitText();
		PromocodeOutput promocode = null;

		try
		{
			using(var client = new HttpClient())
			{
				var endPoint = new Uri($"{_configuration["apiUrl"]}/promocode/get/{promocodeFromUser}");
				var json = client.GetAsync(endPoint).Result.Content.ReadAsStringAsync().Result;
				promocode = JsonConvert.DeserializeObject<PromocodeOutput>(json);

				_logger.LogInformation($"PromocodeController(29): Result: {promocode.Name}");
			}

			if(promocode.ActivatedUserId.Contains($"{FromId}"))
			{
				KButton("⬅️назад");
				PushL("К сожалению промокод не действителен либо вы уже вводили этот промокод. Следите за новостями в наших соц.сетях, чтобы не пропустить новый промокод");
				await Send();
			}

			if(promocodeFromUser == promocode.Name && !promocode.ActivatedUserId.Contains($"{FromId}") && promocode != null)
			{
				using(var client = new HttpClient())
				{
					var endPoint = new Uri($"{_configuration["apiUrl"]}/promocode/update/{promocodeFromUser}/{FromId}");
					var result = client.PutAsync(endPoint, new StringContent("")).Result.Content.ReadAsStringAsync().Result;

					_logger.LogInformation($"PromocodeController(40): Result: {result}");
				}

				using(var client = new HttpClient())
				{
					var endPoint = new Uri($"{_configuration["apiUrl"]}/user/update-balance/{FromId}/{int.Parse($"{promocode.Bonus}")}");
					var result = client.PutAsync(endPoint, new StringContent("")).Result.Content.ReadAsStringAsync().Result;

					_logger.LogInformation($"PromocodeController(40): Result: {result}");
				}

				KButton("⬅️назад");
				PushL($"Вы активировали промокод и получили бонус +{promocode.Bonus}грн. /{promocode.Bonus * 1.93}руб.");
				await Send();
			}
		}
		catch(Newtonsoft.Json.JsonReaderException)
		{
			KButton("⬅️назад");
			PushL("К сожалению промокод не действителен либо вы уже вводили этот промокод. Следите за новостями в наших соц.сетях, чтобы не пропустить новый промокод");
			await Send();
		}
	}
}
