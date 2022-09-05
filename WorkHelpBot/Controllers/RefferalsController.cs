using Deployf.Botf;
using Newtonsoft.Json;

using WorkHelpBot.Models.Output;

using WorkHelpBot.Interfaces;

class RefferalsController : BotController
{
	private ILogger<RefferalsController> _logger;
	private IConfiguration _configuration;
	private IEncodingService _encodingService;

	public RefferalsController(ILogger<RefferalsController> logger, IConfiguration configuration, IEncodingService encodingService)
	{
		_configuration = configuration;
		_logger = logger;
		_encodingService = encodingService;
	}

	[Action("🫂реферальная программа🫂")]
	public async Task Refferals()
	{
		string referNickname = "";
		string referName = "";
		List<RefferalOutput> refferals = null;
		string refferalsStr = "";
		
		try
		{
			using(var client = new HttpClient())
			{
				var endpoint = new Uri($"{_configuration["apiUrl"]}/user/get/{FromId}");
				var jsonUser = client.GetAsync(endpoint).Result.Content.ReadAsStringAsync().Result;
				var user = JsonConvert.DeserializeObject<UserOutput>(jsonUser);

				referNickname = user.RefferNicknameTelegram;

				endpoint = new Uri($"{_configuration["apiUrl"]}/user/get/{user.RefferUserIdTelegram}");
				jsonUser = client.GetAsync(endpoint).Result.Content.ReadAsStringAsync().Result;
				user = JsonConvert.DeserializeObject<UserOutput>(jsonUser);

				referName = user.Name;
			}
		}
		catch(NullReferenceException)
		{
			referNickname = "";
			referName =  "";
		}

		try
		{
			using(var client = new HttpClient())
			{
				var endpoint = new Uri($"{_configuration["apiUrl"]}/refferal/get/{FromId}");
				var jsonRefferals = client.GetAsync(endpoint).Result.Content.ReadAsStringAsync().Result;
				refferals = JsonConvert.DeserializeObject<IEnumerable<RefferalOutput>>(jsonRefferals).ToList();
			}
		}
		catch(NullReferenceException e)
		{
			refferals = new List<RefferalOutput>();
		}
		catch(ArgumentNullException e)
		{
			refferals = new List<RefferalOutput>();
		}

		foreach(var item in refferals)
		{
			refferalsStr += $"@{item.RefferalNicknameTelegram} | {item.Earned}грн. /{item.Earned * 1.93}руб. заработано\n";
		}

		KButton("⬅️назад");
		PushL($"Твой рефер: @{referNickname} | {await _encodingService.Decode(referName)}\n\nТи можешь пригласить друга и заработать больше денег. Твой процент за реферала составляет: {_configuration["refferalProcent"]}%\n\nТвоя ссылка приглашения друзей:\n<a href='https://t.me/money_provided_bot?start={FromId}'>Тебе нужны деньги ? Без проблем -> Нажимай на текст и кликай /start в боте и зарабатывай деньги выполняя простые задания. Ти не можешь пропустить это предложение, так как мы платим +10% людям, которые кликнут именно по этой ссфлке\n\nНажми на меня и погнали зарабатывать реальные бабки 😉</a>\n\nТвои рефералы: \n{refferalsStr}");
		await Send();
	}
}
