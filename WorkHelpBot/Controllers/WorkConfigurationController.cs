using Deployf.Botf;
using Newtonsoft.Json;

using WorkHelpBot.Models.Input;
using WorkHelpBot.Models.Output;

using WorkHelpBot.Interfaces;

public class WorkConfigurationController : BotController
{
	private IConfiguration _configuration;
	private ILogger<WorkConfigurationController> _logger;
	private IEncodingService _encodingService;

	public WorkConfigurationController(IConfiguration configuration, ILogger<WorkConfigurationController> logger, IEncodingService encodingService)
	{
		_configuration = configuration;
		_logger = logger;
		_encodingService = encodingService;
	}

	[Authorize("admin")]
	[Action("/add-work")]
	public async Task AddWork()
	{
		PushL("Введите категорию работы: ");
		await Send();
		string category = await _encodingService.Encode(await AwaitText());

		PushL("Введите название работы: ");
		await Send();
		string name = await _encodingService.Encode(await AwaitText());

		PushL("Введите описание работы: ");
		await Send();
		string desc = await _encodingService.Encode(await AwaitText());

		PushL("Введите цену работы: ");
		await Send();
		int price = Convert.ToInt32(await AwaitText());

		PushL("Введите ссылку на пост: ");
		await Send();
		string link = await AwaitText();

		using(var client = new HttpClient())
		{
			var endPoint = new Uri($"{_configuration["apiUrl"]}/work/add");
			var jsonWork = JsonConvert.SerializeObject(new WorkInput { Category = category, Name = name, Description = desc, Price = price, Link = link });
			var payload = new StringContent(jsonWork, System.Text.Encoding.UTF8, "application/json");
			var result = client.PostAsync(endPoint, payload).Result.Content.ReadAsStringAsync().Result;

			_logger.LogInformation($"WorkConfigurationController(41): /add-work: Result: {result}");
		}

		PushL("Работа добавлена");
		await Send();
	}

	[Authorize("admin")]
	[Action("/delete-work")]
	public async Task DeleteWork()
	{
		PushL("Введите название работы: ");
		await Send();
		var name = await _encodingService.Encode(await AwaitText());

		using(var client = new HttpClient())
		{
			var endPoint = new Uri($"{_configuration["apiUrl"]}/work/delete/{name}");
			var result = client.DeleteAsync(endPoint).Result;

			_logger.LogInformation($"WorkConfigurationController(63): /delete-work: Result: {result}");
		}

		PushL("Работа удалена");
		await Send();
	}

	[Authorize("admin")]
	[Action("/update-work")]
	public async Task UpdateWork()
	{
		PushL("Введите категорию работы: ");
		await Send();
		string category = await _encodingService.Encode(await AwaitText());

		PushL("Введите название работы: ");
		await Send();
		string name = await _encodingService.Encode(await AwaitText());

		PushL("Введите описание работы: ");
		await Send();
		string desc = await _encodingService.Encode(await AwaitText());

		PushL("Введите цену работы: ");
		await Send();
		int price = Convert.ToInt32(await AwaitText());

		PushL("Введите ссылку на пост: ");
		await Send();
		string link = await AwaitText();

		using(var client = new HttpClient())
		{
			var endPoint = new Uri($"{_configuration["apiUrl"]}/work/update");
			var jsonWork = JsonConvert.SerializeObject(new WorkInput { Category = category, Name = name, Description = desc, Price = price, Link = link });
			var payload = new StringContent(jsonWork, System.Text.Encoding.UTF8, "application/json");
			var result = client.PutAsync(endPoint, payload).Result.Content.ReadAsStringAsync().Result;

			_logger.LogInformation($"WorkConfigurationController(98): /udate-work: Result: {result}");
		}

		PushL("Работа обновлена");
		await Send();
	}

	[Authorize("admin")]
	[Action("/works")]
	public async Task Works()
	{
		List<WorkOutput> works = null;

		using(var client = new HttpClient())
		{
			var endPoint = new Uri($"{_configuration["apiUrl"]}/work/works");
			var json = client.GetAsync(endPoint).Result.Content.ReadAsStringAsync().Result;
			works = JsonConvert.DeserializeObject<List<WorkOutput>>(json);

			_logger.LogInformation($"WorkConfigurationController(122): Result: {works}");
		}

		PushL("Все работы: \n");

		foreach(var work in works)
		{
			PushL($"Category: {await _encodingService.Decode(work.Category)} | Name: {await _encodingService.Decode(work.Name)} | Description: {await _encodingService.Decode(work.Description)} | Price: {work.Price} | Link: {work.Link}\n");
		}
		await Send();
	}
}



