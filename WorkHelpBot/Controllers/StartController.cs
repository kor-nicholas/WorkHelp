using Deployf.Botf;
using Telegram.Bot;
using Newtonsoft.Json;

using System;
using System.Text;
using System.Security.Cryptography;
using System.Reflection;
using System.Reflection.Emit;

using WorkHelpBot.Models;
using WorkHelpBot.Models.Input;
using WorkHelpBot.Models.Output;

using WorkHelpBot.Interfaces;

class StartController : BotController
{
	readonly ILogger _logger;
	readonly IConfiguration _configuration;
	readonly IEncodingService _encodingService;

	public StartController(ILogger<StartController> logger, IConfiguration configuration, IEncodingService encodingService)
	{
		_logger = logger;
		_configuration = configuration;
		_encodingService = encodingService;
	}

	
	[Action("/start", "Start Bot")]
	public async Task Start()
	{
		try
		{
			UserOutput user = await GetUserForUserId(FromId);
			await Login(user);
		}
		catch(Newtonsoft.Json.JsonReaderException e)
		{
			await Registration();
			return;
		}
	}

	[Action("/start")]
	public async Task Start(long userId)
	{
		try
		{
			UserOutput user = await GetUserForUserId(FromId);
			await Login(user);
		}
		catch(Newtonsoft.Json.JsonReaderException)
		{
			await Registration();
			
			var reffer = await Client.GetChatMemberAsync(_configuration["channelId"], userId);

			using(var client = new HttpClient())
			{
				var endPoint = new Uri($"{_configuration["apiUrl"]}/user/add-reffer-data");
				var jsonRefferData = JsonConvert.SerializeObject(new RefferalInput { RefferalUserIdTelegram = $"{FromId}", RefferUserIdTelegram = $"{userId}", RefferNicknameTelegram = $"{reffer.User.Username}"});

				var payload = new StringContent(jsonRefferData, System.Text.Encoding.UTF8, "application/json");
				var result = client.PostAsync(endPoint, payload).Result.Content.ReadAsStringAsync().Result;

				_logger.LogInformation($"StartController(55): Result: {result}");
			}

			await Client.SendTextMessageAsync(_configuration["baseId"], $"[Refferals] New refferal: \n@{Context.GetUsername()} ({FromId}) is new refferal for @{reffer.User.Username} ({userId})");
			return;
		}
		catch(Exception e)
		{
			_logger.LogCritical(e.Message);
		}
	}

	// Additional methods
	[Action]
	private async Task<UserOutput> GetUserForUserId(long FromId)
	{
		using(var client = new HttpClient())
		{
			var endPoint = new Uri($"{_configuration["apiUrl"]}/user/get/{FromId}");
			var json = client.GetAsync(endPoint).Result.Content.ReadAsStringAsync().Result;
			var result = JsonConvert.DeserializeObject<UserOutput>(json);

			_logger.LogInformation($"StartController(105): Result: {result.Name}");

			return result;
		}
	}

	[Action]
	private async Task Registration()
	{
		PushL("Введите ваше имя: ");
		await Send();
		var name = await _encodingService.Encode(await AwaitText());

		PushL("Введите вашу фамилию: ");
		await Send();
		var surName = await _encodingService.Encode(await AwaitText());

		PushL("Введите ваш номер телефона: ");
		await Send();
		var phone = await AwaitText();
		
		if(!phone.Contains("+"))
		{
			Button("Повторить попытку");
			PushL("Формат телефона введен неверно. Проверьте попытку еще раз");
			await Send();

			var callPhone = await AwaitQuery();

			if(callPhone == "Повторить попытку")
			{
				await Registration();
				return;
			}
		}

		PushL("Введите ваш email: ");
		await Send();
		var email = await AwaitText();
		
		if(!email.Contains("@"))
		{
			Button("Повторить попытку");
			PushL("Формат почты введен неверно. Проверьте попытку еще раз");
			await Send();

			var callEmail = await AwaitQuery();

			if(callEmail == "Повторить попытку")
			{
				await Registration();
				return;
			}
		}

		PushL("Придумайте логин: ");
		await Send();
		var login = await AwaitText();

		PushL("Придумайте пароль (минимум 8 символов): ");
		await Send();
		var pass = await AwaitText();

		if(pass.Length < 8)
		{
			Button("Повторить попытку");
			PushL("Минимальная длина пароля 8 символов. Попробуйте еще раз");
			await Send();

			var againCallBack = await AwaitQuery();

			if(againCallBack == "Повторить попытку")
			{
				await Registration();
				return;
			}
		}

		using(var client = new HttpClient())
		{
			var endPoint = new Uri($"{_configuration["apiUrl"]}/user/add");
			var jsonUser = JsonConvert.SerializeObject(new UserInput { Name = name, Surname = surName, Phone = phone, Email = email, UserIdTelegram = $"{FromId}", NicknameTelegram = Context.GetUsername(), Pass = pass, Login = login, Role = "user", Balance = 0, Salt = "", RefferUserIdTelegram = "", RefferNicknameTelegram = "", CompletedTaskCount = 0, RefferalsCount = 0 });
			var payload = new StringContent(jsonUser, System.Text.Encoding.UTF8, "application/json");
			var result = client.PostAsync(endPoint, payload).Result.Content.ReadAsStringAsync().Result;

			_logger.LogInformation($"StartController(129): Result: {result}");
		}

		PushL("Спасибо за регистрацию");
		await Send();

		UserOutput user = await GetUserForUserId(FromId);
		await Login(user);
	}

	[Action]
	private async Task Login(UserOutput user)
	{
		PushL("Авторизируйтесь в системе");
		await Send();

		PushL("Введите ваш логин: ");
		await Send();
		var login = await AwaitText();

		PushL("Введите ваш пароль: ");
		await Send();
		var pass = await AwaitText();

		if(pass.Length < 8)
		{
			Button("Повторить попытку");
			PushL("Минимальная длина пароля 8 символов. Попробуйте еще раз");
			await Send();

			var againCallBack = await AwaitQuery();

			if(againCallBack == "Повторить попытку")
			{
				await Login(user);
				return;
			}
		}

		int index = Convert.ToInt32($"{user.Salt[user.Salt.Length - 2]}{user.Salt[user.Salt.Length - 1]}");
		pass = pass.Insert(index, user.Salt);
		pass = await HashPass(pass);

		if(user.Login == login && user.Pass == pass && user.UserIdTelegram == $"{FromId}")
		{
			await CheckSubscribe(FromId);
		}
		else
		{
			Button("Повторить попытку");
			PushL("Login или пароль введены неверно (либо вы пытаетесь войти в аккаунт с чужого Телеграм аккаунта). Проверьте правильность ваших данных и повторите попытку");
			await Send();

			var againCallBack = await AwaitQuery();

			if(againCallBack == "Повторить попытку")
			{
				await Login(user);
				return;
			}
		}
	}

	[Action]
	private async Task<string> HashPass(string pass)
	{
		using(var sha256 = SHA256.Create())
		{
			var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));
			var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

			_logger.LogInformation($"Hash: {hash}");

			return hash;
		}
	}

	[Action]
	private async Task CheckSubscribe(long FromId)
	{
		var chatMember = await Client.GetChatMemberAsync(_configuration["channelId"], FromId);
		string status = chatMember.Status.ToString();
		
		do
		{
			Button("Подписаться на канал", "https://t.me/free_films_every_day");
			Button("Я подписался");
			PushL("Подпишись на канал и попробуй еще раз");
			await Send();
				
			var callSubscribe = await AwaitQuery();

			if(callSubscribe == "Я подписался")
			{
				chatMember = await Client.GetChatMemberAsync(_configuration["channelId"], FromId);
				status = chatMember.Status.ToString();
			}
		}while(status == "Left");
		
		KButton("👤профиль👤");
		RowKButton("💸заработок💸");
		KButton("❗️помощь❗️");
		RowKButton("💳вывод💳");
		KButton("🎟промокод🎟");
		RowKButton("🫂реферальная программа🫂");
		PushL("Привет друг👋. Я смотрю ты тут новенький, давай расскажу что к чему:\n\n🤑С помощью нашего бота, ты сможешь обеспечить себя дополнительным заработком, просто лёжа на диване;\n\n❗️Это не очередной развод или скам. Вы несёте нам деньги от наших партнёров - мы отдаём вам большой процент. Потому все остаются в выгоде;\n\n😍С нами ты будешь на шаг впереди к своей мечте, а может даже и не к одной!");
		await Send();
	}

	[On(Handle.Unauthorized)]
	public async Task Unauthorized()
	{
		_logger.LogError("StartController(254): Error with Authorizing user");
		PushL("Вы не зарегистрированы в системе. Зарегистрируйтесь и тогда продолжите работу");
		await Send();

		await Registration();
	}
}



