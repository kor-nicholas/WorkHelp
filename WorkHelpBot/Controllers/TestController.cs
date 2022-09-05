using Deployf.Botf;
using Newtonsoft.Json;

using System;
using System.Text;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;

using WorkHelpBot.Models.Output;
using WorkHelpBot.Models.Input;

using WorkHelpBot.Interfaces;

public class TestController : BotController
{
	readonly ILogger<TestController> _logger;
	readonly IConfiguration _configuration;
	readonly IEncodingService _encodingService;

	public TestController(ILogger<TestController> logger, IConfiguration configuration, IEncodingService encodingService)
	{
		_logger = logger;
		_configuration = configuration;
		_encodingService = encodingService;
	}

	[Action("/test1")]
	public async Task Test1()
	{
		string[] strs = {"Test str 1", "Тестовая строка 2"};

		string str = await _encodingService.Encode(strs[1]);
		PushL(str);
		await Send();

		PushL(await _encodingService.Decode(str));
	}

	[Action("/test2")]
	public async Task Test2()
	{
		PushL("1 - true, 0 - false -> ");
		await Send();

		var chose = await AwaitText();

		if(chose == "1")
		{
			await Test21();
		}
		else
		{
			await Test21();
			return;
		}

		PushL("finish");
		await Send();
	}

	[Action]
	public async Task Test21()
	{
		PushL("test21");
		await Send();
	}

	[Action("/test3")]
	public async Task Test3()
	{
		using(var client = new HttpClient())
		{
			var endpoint = new Uri($"{_configuration["apiUrl"]}/user/add-reffer-data");
			var jsonRefferData = JsonConvert.SerializeObject(new RefferalInput { RefferalUserIdTelegram = "596900840", RefferUserIdTelegram = $"{FromId}", RefferNicknameTelegram = "kornic145" });
			var payload = new StringContent(jsonRefferData, System.Text.Encoding.UTF8, "application/json");
			var result = client.PostAsync(endpoint, payload).Result.Content.ReadAsStringAsync().Result;

			_logger.LogInformation($"Result: {result}");
		}
	}
}




