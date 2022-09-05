using Deployf.Botf;
using Newtonsoft.Json;

using Telegram.Bot;
using Telegram.Bot.Types.Enums;

using WorkHelpBot.Models.Output;
using WorkHelpBot.Models.Input;

using WorkHelpBot.Interfaces;

class WorkController : BotController
{
	readonly ILogger _logger;
	readonly IConfiguration _configuration;
	readonly MessageSender _sender;
	readonly IEncodingService _encodingService;
	
	public WorkController(ILogger<WorkController> logger, IConfiguration configuration, MessageSender sender, IEncodingService encodingService)
	{
		_logger = logger;
		_configuration = configuration;
		_sender = sender;
		_encodingService = encodingService;
	}

	[Action]
	public async Task Main()
	{
		KButton("👤профиль👤");
		RowKButton("💸заработок💸");
		KButton("❗️помощь❗️");
		RowKButton("💳вывод💳");
		KButton("🎟промокод🎟");
		RowKButton("🫂реферальная программа🫂");
		PushL("Ты находишься в главном меню:");
		await Send();
	}
	
	[Action("💸заработок💸")]
	public async Task Work()
	{
		KButton("⬅️назад");
		PushL("Чтобы вернуться назад, нажми на кнопку: ");
		await Send();

		Button("Регистрация");
		/* Button("Игры"); */
		/* RowButton("Банковские карты"); */
		Button("Кредиты");
		/* RowButton("Верификация"); */
		RowButton("Вводний урок");
		Button("Redsurf");
		PushL("Выберите тип заработка:");
		await Send();

		var update = await AwaitNextUpdate();

		if(update.Update.Type == UpdateType.CallbackQuery)
		{
			var works = await SetWorks();
			works = works.Where(work => work.Category == $"{update.Update.CallbackQuery.Data.ToString()}").ToList();
				
			foreach(var work in works)
			{
				AState(work);
				Button("Приступить к выполнению", Q(CallbackWorkHandler, work));
				PushL($"<b>{work.Name}</b>\n\n{work.Description}\n\nОплата: {work.Price}грн. /{work.Price * 1.93}руб.");
				await Send();
			}
		}
		else if(update.Update.Type == UpdateType.Message)
		{
			await Main();
		}
	}
		
	[Action]
	public async Task Registration([State] WorkOutput work)
	{
		await AState(work);

		Button("Перейти к посту", work.Link);
		RowButton("Я зарегистрировался");
		RowButton("Отменить задание");
		PushL("Перейдите к посту и зарегистрируйтесь по ссылке");
		await Send();

		var callRegisterInRegistration = await AwaitQuery();

		if (callRegisterInRegistration == "Я зарегистрировался")
		{
			string email = null;
			string nameSurname = null;
			int age = 0;

			PushL("Введите почту, указанную при регистрации на сайте: ");
			await Send();
			email = await AwaitText();

			if(!email.Contains("@"))
			{
				Button("Повторить попытку");
				PushL("Формат почты введен неверно");
				await Send();

				var callEmail = await AwaitQuery();

				if(callEmail == "Повторить попытку")
				{
					await Registration(work);
					return;
				}
			}

			PushL("Введите имя и фамилию(через пробел), указанные при регистрации на сайте: ");
			await Send();
			nameSurname = await AwaitText();

			PushL("Введите возраст, указанный при регистрации на сайте: ");
			await Send();

			try
			{
				age = int.Parse(await AwaitText());
			}
			catch(FormatException)
			{
				Button("Повторить попытку");
				PushL("Возраст должен быть числом");
				await Send();

				var callAge = await AwaitQuery();

				if(callAge == "Повторить попытку")
				{
					await Registration(work);
					return;
				}
			}

			await AState(work);

			var msg = new MessageBuilder()
					.Button("Принять оплату ✅", Q(CallbackAcceptWorkHandler, "yes", FromId, work.Price))
					.Button("Отклонить оплату ❌", Q(CallbackAcceptWorkHandler, "no", FromId, work.Price))
					.SetChatId(long.Parse(_configuration["baseId"]))
					.PushL($"[Registration] @{Context.GetUsername()} registered in familiarity\n\nName and Surname: {nameSurname}\nAge: {age}\nEmail: {email}");

			await _sender.Send(msg);

			KButton("⬅️назад");
			PushL("Ваша заявка на получение оплаты за задание отправлена. Подождите некоторое время (до 24 часов), пока наш менеджер проверит вашу заявку\n\nСейчас вы можете вернуться в главное меню и выполнить остальные задания");
			await Send();
		}
		else if(callRegisterInRegistration == "Отменить задание")
		{
			await Main();
		}
		else
		{
			throw new Exception("WorkController(87): Problem with callback");
		}
	}

	[Action]
	public async Task BankCards([State] WorkOutput work)
	{
		KButton("⬅️назад");
		PushL("Данный раздел находиться в разработке");
		await Send();
	}

	[Action]
	public async Task Verification([State] WorkOutput work)
	{
		/* Button("Перейти к посту", "https://t.me/free_films_every_day/58"); */
		/* RowButton("Я зарегистрировался"); */
		/* PushL("Перейдите к посту и зарегистрируйтесь по ссылке"); */
		/* await Send(); */

		/* var callRegister = await AwaitQuery(); */

		/* if (callRegister == "Я зарегистрировался") */
		/* { */
		/* 	Button("Посмотреть мануал", "https://telegra.ph"); */
		/* 	RowButton("Я закончил верификацию"); */
		/* 	PushL("Прийдите верификацию. Больше информации вы сможете найти в мануале"); */
		/* 	await Send(); */

		/* 	var callVarification = await AwaitQuery(); */

		/* 	if (callVarification == "Я закончил верификацию") */
		/* 	{ */
		/* 		PushL("Введите данные почты (в формате login:pass): "); */
		/* 		await Send(); */

		/* 		var dataEmail = await AwaitText(); */

		/* 		PushL("Введите данные CoinList(в формате login:pass): "); */
		/* 		await Send(); */

		/* 		var dataCoinList = await AwaitText(); */

		/* 		var chatMember = await Client.GetChatMemberAsync(-1001696554718, FromId); */
		/* 		await Client.SendTextMessageAsync(-726773196, $"[Verification] @{chatMember.User.Username} complete verification\n\nEmail: {dataEmail}\nCoinList: {dataCoinList}"); */
		/* 	} */
		/* } */

		/* KButton("Назад"); */
		/* PushL("Благодарим за выполненную работу!\n\nВы можете вернуться в главное меню и заработать больше денег!"); */

		KButton("⬅️назад");
		PushL("Данный раздел находиться в разработке");
		await Send();
	}

	[Action]
	public async Task Credits([State] WorkOutput work)
	{
		await AState(work);

		Button("Перейти к посту", work.Link);
		RowButton("Я оформил кредит и получил деньги");
		RowButton("Отменить задание");
		PushL("Прийдите к посту и оформите кредит");
		await Send();

		var callCredit = await AwaitQuery();

		if(callCredit == "Я оформил кредит и получил деньги")
		{
			string nameAndSurname = null;
			string phone = null;
			string email = null;

			PushL("Введите фамилию и имя(через пробел), указанные в заявке: ");
			await Send();
			nameAndSurname = await AwaitText();

			PushL("Введите телефон, указанный в заявке: ");
			await Send();
			phone = await AwaitText();

			if(!phone.Contains("+"))
			{
				Button("Повторить попытку");
				PushL("Формат телефона введен неверно");
				await Send();

				var callPhone = await AwaitQuery();

				if(callPhone == "Повторить попытку")
				{
					await Credits(work);
					return;
				}
			}

			PushL("Введите почту, указанную в заявке: ");
			await Send();
			email = await AwaitText();

			if(!email.Contains("@"))
			{
				Button("Повторить попытку");
				PushL("Формат почты введен неверно");
				await Send();

				var callEmail = await AwaitQuery();

				if(callEmail == "Повторить попытку")
				{
					await Credits(work);
					return;
				}
			}

			await AState(work);

			var msg = new MessageBuilder()
					.Button("Принять оплату ✅", Q(CallbackAcceptWorkHandler, "yes", FromId, work.Price))
					.Button("Отклонить оплату ❌", Q(CallbackAcceptWorkHandler, "no", FromId, work.Price))
					.SetChatId(long.Parse(_configuration["baseId"]))
					.PushL($"[Credits] @{Context.GetUsername()} take credit: \n\nName and Surname: {nameAndSurname}\nPhone: {phone}\nEmail: {email}");
			await _sender.Send(msg);

			KButton("⬅️назад");
			PushL("Ваша заявка на получение оплаты за задание отправлена. Подождите некоторое время (до 24 часов), пока наш менеджер проверит вашу заявку\n\nСейчас вы можете вернуться в главное меню и выполнить остальные задания");
			await Send();
		}
		else if(callCredit == "Отменить задание")
		{
			await Main();
		}
		else
		{
			throw new Exception("WorkController(187): Problem with callback");
		}
	}

	[Action]
	public async Task Study([State] WorkOutput work)
	{
		await AState(work);

		Button("Перейти к посту", work.Link);
		RowButton("Я записался на вводный урок");
		RowButton("Отменить задание");
		PushL("Прийдите к посту и запишитесь на вводный урок");
		await Send();

		var callStudy = await AwaitQuery();

		if(callStudy == "Я записался на вводный урок")
		{
			string name = null;
			string phone = null;
			string email = null;

			PushL("Введите имя, указанное в заявке: ");
			await Send();
			name = await AwaitText();

			PushL("Введите телефон, указанный в заявке: ");
			await Send();
			phone = await AwaitText();

			if(!phone.Contains("+"))
			{
				Button("Повторить попытку");
				PushL("Формат телефона введен неверно");
				await Send();

				var callPhone = await AwaitQuery();

				if(callPhone == "Повторить попытку")
				{
					await Study(work);
					return;
				}
			}

			PushL("Введите почту, указанную в заявке: ");
			await Send();
			email = await AwaitText();

			if(!email.Contains("@"))
			{
				Button("Повторить попытку");
				PushL("Формат почты введен неверно");
				await Send();

				var callEmail = await AwaitQuery();

				if(callEmail == "Повторить попытку")
				{
					await Study(work);
					return;
				}
			}

			await Client.SendTextMessageAsync(_configuration["baseId"], $"[Study] @{Context.GetUsername()} signed up for course: \n\nName: {name}\nPhone: {phone}\nEmail: {email}");
			PushL("Отлично. Теперь пройдите вводный урок c преподователем и отправьте команду /sendstudy, чтобы получить оплату (если хотите - можете приобрести курс и получить скидку 10%)");
			await Send();
		}
		else if(callStudy == "Отменить задание")
		{
			await Main();
		}
		else
		{
			throw new Exception("WorkController(238): Problem with callback");
		}
	}

	[Action("/sendstudy")]
	public async Task SendStudy()
	{
		var msg = new MessageBuilder()
			.Button("Принять оплату ✅", Q(CallbackAcceptWorkHandler, "yes", FromId, 14))
			.Button("Отклонить оплату ❌", Q(CallbackAcceptWorkHandler, "no", FromId, 14))
			.SetChatId(long.Parse(_configuration["baseId"]))
			.PushL($"[Study] @{Context.GetUsername()} passed trial lesson");

		await _sender.Send(msg);

		KButton("⬅️назад");
		PushL("Ваша заявка на получение оплаты за задание отправлена. Подождите некоторое время (до 24 часов), пока наш менеджер проверит вашу заявку\n\nСейчас вы можете вернуться в главное меню и выполнить остальные задания");
		await Send();
	}

	[Action]
	public async Task GamesAndApps([State] WorkOutput work)
	{
		AState(work);

		Button("Перейти к посту", work.Link);
		RowButton("Я зарегистрировался(лась)");
		RowButton("Отменить задание");
		PushL("Прийдите к посту и зарегистрируйтесь в браузерной игре");
		await Send();

		var callGame = await AwaitQuery();

		if(callGame == "Я зарегистрировался(лась)")
		{
			string nickName = null;
			string email = null;

			PushL("Введите никнейм, указанный при регистрации: ");
			await Send();
			nickName = await AwaitText();

			PushL("Введите почту, указанную при регистрации: ");
			await Send();
			email = await AwaitText();

			if(!email.Contains("@"))
			{
				Button("Повторить попытку");
				PushL("Формат почты введен неверно");
				await Send();

				var callEmail = await AwaitQuery();

				if(callEmail == "Повторить попытку")
				{
					await GamesAndApps(work);
					return;
				}
			}

			await Client.SendTextMessageAsync(_configuration["baseId"], $"[GamesAndApps] @{Context.GetUsername()} signed up in game: \n\nNickName: {nickName}\nEmail: {email}");
			PushL("Отлично. Теперь поиграйте в игру на протияжении 7 дней и отправьте скриншот профиля боту (главное чтобы было видно дату регистрации и никнейм пользователя). Используйте команду /sendphotogame, когда будете готовы отправить скриншот");
			await Send();
		}
		else if(callGame == "Отменить задание")
		{
			await Main();
		}
		else
		{
			throw new Exception("WorkController(303): Problem with callback");
		}
	}

	[Action("/sendphotogame")]
	public async Task SendPhotoGame()
	{
		KButton("⬅️назад");
		PushL("Отправьте скриншот профиля, где видно дату регистрации и никнейм пользователя");
		await Send();

		var update = await AwaitNextUpdate();

		if(update.Update.Message.Photo[0].FileId != "")
		{
			var msg = new MessageBuilder()
				.Button("Принять оплату ✅", Q(CallbackAcceptWorkHandler, "yes", FromId, 15))
				.Button("Отклонить оплату ❌", Q(CallbackAcceptWorkHandler, "no", FromId, 15))
				.SetChatId(long.Parse(_configuration["baseId"]))
				.PushL($"[GamesAndApps] @{Context.GetUsername()} complete play for 7 days")
				.SetPhotoUrl(update.Update.Message.Photo[0].FileId);

			await _sender.Send(msg);

			KButton("⬅️назад");
			PushL("Ваша заявка на получение оплаты за задание отправлена. Подождите некоторое время (до 24 часов), пока наш менеджер проверит вашу заявку\n\nСейчас вы можете вернуться в главное меню и выполнить остальные задания");
			await Send();
		}
		else
		{
			KButton("⬅️назад");
			PushL("Это не фотография. Попробуйте еще раз");
			await Send();
		}
	}

	[Action]
	public async Task Redsurf([State] WorkOutput work)
	{
		AState(work);

		RowButton("Я зарегистрировался(лась)");
		RowButton("Отменить задание");
		PushL("Зарегистрируйтесь на <a href='http://redsurf.ru/?r=334692'>сайте</a>");
		await Send();

		var callRedsurf = await AwaitQuery();

		if(callRedsurf == "Я зарегистрировался(лась)")
		{
			PushL("Введите никнейм/email, указанный при регистрации: ");
			await Send();
			var nickNameOrEmail = await AwaitText();

			await Client.SendTextMessageAsync(_configuration["baseId"], $"[Redsurf] @{Context.GetUsername()} signed up in Redsurf: \n\nNickName/Email: {nickNameOrEmail}");
			PushL("Отлично. Теперь наберите какое-то количество кредитов и введите команду /sendredsurf, чтобы отправить данные и получить оплату (стоимость закупки кредитов: 0.01грн./0.0193руб. за 1 кредит, минимальный заказ - 100 кредитов(больше - количество должно быть кратным 100))");
			await Send();
		}
		else if(callRedsurf == "Отменить задание")
		{
			await Main();
		}
		else
		{
			throw new Exception("WorkController(372): Problem with callback");
		}
	}

	[Action("/sendredsurf")]
	public async Task SendRedsurf()
	{
		PushL("Отправьте данные аккаунты и получите оплату");
		await Send();

		PushL("Введите login/email: ");
		await Send();
		var loginOrEmail = await AwaitText();

		PushL("Введите пароль: ");
		await Send();
		var pass = await AwaitText();

		PushL("Введите количество кредитов(число должно быть кратным 100): ");
		await Send();

		int countCredits = 0;

		try
		{
			countCredits = int.Parse(await AwaitText());
		}
		catch(FormatException)
		{
			KButton("⬅️назад");
			PushL("Количество кредитов должно быть числом. Попробуйте ввести команду /sendredsurf и повторите попытку еще раз");
			await Send();
			return;
		}

		if(countCredits % 100 != 0)
		{
			KButton("⬅️назад");
			PushL("Число не кратное 100 (100, 200, 300, ...). Наберите минимум 100 кредитов, введите снова команду /sendredsurf и повторите попытку еще раз");
			await Send();
			return;
		}
		else
		{
			int price = countCredits / 100;

			var msg = new MessageBuilder()
				.Button("Принять оплату ✅", Q(CallbackAcceptWorkHandler, "yes", FromId, price))
				.Button("Отклонить оплату ❌", Q(CallbackAcceptWorkHandler, "no", FromId, price))
				.SetChatId(long.Parse(_configuration["baseId"]))
				.PushL($"[Redsurf] @{Context.GetUsername()} want sell {countCredits} for {price}грн. / {price * 1.93}руб. ({loginOrEmail}:{pass})");

			await _sender.Send(msg);

			KButton("⬅️назад");
			PushL("Ваша заявка на получение оплаты за задание отправлена. Подождите некоторое время (до 24 часов), пока наш менеджер проверит вашу заявку\n\nСейчас вы можете вернуться в главное меню и выполнить остальные задания");
			await Send();
		}
	}

	// More methods
	[Action]
	public async Task<List<WorkOutput>> SetWorks()
	{
		using(var client = new HttpClient())
		{
			try
			{
				var endPoint = new Uri($"{_configuration["apiUrl"]}/work/works");
				var json = client.GetAsync(endPoint).Result.Content.ReadAsStringAsync().Result;
				var result = JsonConvert.DeserializeObject<List<WorkOutput>>(json);

				foreach(var item in result)
				{
					item.Category = await _encodingService.Decode(item.Category);
					item.Name = await _encodingService.Decode(item.Name);
					item.Description = await _encodingService.Decode(item.Description);
				}

				return result;
			}
			catch(NullReferenceException)
			{
				return new List<WorkOutput>();
			}
			catch(ArgumentNullException)
			{
				return new List<WorkOutput>();
			}
		}
	}
	
	[Action]
	public async Task CallbackWorkHandler([State] WorkOutput work)
	{
		if(work.Category == "Регистрация")
		{
			await Registration(work);
		}
		else if(work.Category == "Банковские карты")
		{
			await BankCards(work);
		}
		else if(work.Category == "Верификация")
		{
			await Verification(work);
		}
		else if(work.Category == "Кредиты")
		{
			await Credits(work);
		}
		else if(work.Category == "Вводний урок")
		{
			await Study(work);
		}
		else if(work.Category == "Игры")
		{
			await GamesAndApps(work);
		}
		else if(work.Category == "Redsurf")
		{
			await Redsurf(work);
		}
		else
		{
			_logger.LogError("WorkController(304): CallbackWorkHandler: callback not find");
		}
	}

	[Action]
	public async Task CallbackAcceptWorkHandler(string accept, long userId, int price)
	{
		if(accept == "yes")
		{
			UserOutput user = null;
			UserOutput refer = null;

			using(var client = new HttpClient())
			{
				var endPoint = new Uri($"{_configuration["apiUrl"]}/user/get/{userId}");
				var json = client.GetAsync(endPoint).Result.Content.ReadAsStringAsync().Result;
				user = JsonConvert.DeserializeObject<UserOutput>(json);

				_logger.LogInformation($"WorkController(351): Result: {user}");

				if(user.RefferUserIdTelegram != "" && user.RefferNicknameTelegram != "")
				{
					endPoint = new Uri($"{_configuration["apiUrl"]}/user/get/{user.RefferUserIdTelegram}");
					json = client.GetAsync(endPoint).Result.Content.ReadAsStringAsync().Result;
					refer = JsonConvert.DeserializeObject<UserOutput>(json);

					_logger.LogInformation($"WorkController(359): Result: {refer}");
				}
			}

			_logger.LogInformation($"price: {price}");

			using(var client = new HttpClient())
			{
				var endPoint = new Uri($"{_configuration["apiUrl"]}/user/update");
				var jsonUser = JsonConvert.SerializeObject(new UserOutput {Name = user.Name, Surname = user.Surname, Phone = user.Phone, Email = user.Email, UserIdTelegram = user.UserIdTelegram, NicknameTelegram = user.NicknameTelegram, Pass = user.Pass, Login = user.Login, Role = user.Role, Balance = user.Balance + price, Salt = user.Salt, RefferUserIdTelegram = user.RefferUserIdTelegram, RefferNicknameTelegram = user.RefferNicknameTelegram, CompletedTaskCount = user.CompletedTaskCount + 1, RefferalsCount = user.RefferalsCount});
				var payload = new StringContent(jsonUser, System.Text.Encoding.UTF8, "application/json");
				var result = client.PutAsync(endPoint, payload).Result.Content.ReadAsStringAsync().Result;

				_logger.LogInformation($"WorkController(371): Result: {result}");
			}

			if(refer != null)
			{
				using(var client = new HttpClient())
				{
					var endPoint = new Uri($"{_configuration["apiUrl"]}/user/update");
					var jsonUser = JsonConvert.SerializeObject(new UserOutput {Name = refer.Name, Surname = refer.Surname, Phone = refer.Phone, Email = refer.Email, UserIdTelegram = refer.UserIdTelegram, NicknameTelegram = refer.NicknameTelegram, Pass = refer.Pass, Login = refer.Login, Role = refer.Role, Balance = refer.Balance + price * int.Parse(_configuration["refferalProcent"]) / 100, Salt = refer.Salt, RefferUserIdTelegram = refer.RefferUserIdTelegram, RefferNicknameTelegram = refer.RefferNicknameTelegram, CompletedTaskCount = refer.CompletedTaskCount, RefferalsCount = refer.RefferalsCount});
					var payload = new StringContent(jsonUser, System.Text.Encoding.UTF8, "application/json");
					var result = client.PutAsync(endPoint, payload).Result.Content.ReadAsStringAsync().Result;

					_logger.LogInformation($"WorkController(383): Result: {result}");
				}

				using(var client = new HttpClient())
				{
					var endPoint = new Uri($"{_configuration["apiUrl"]}/refferal/add");
					var jsonRefferal = JsonConvert.SerializeObject(new RefferalInput { RefferUserIdTelegram = refer.UserIdTelegram, RefferNicknameTelegram = refer.NicknameTelegram, RefferalUserIdTelegram = user.UserIdTelegram, RefferalNicknameTelegram = user.NicknameTelegram, Earned = price * int.Parse(_configuration["refferalProcent"]) / 100 });
					var payload = new StringContent(jsonRefferal, System.Text.Encoding.UTF8, "application/json");
					var result = client.PostAsync(endPoint, payload).Result.Content.ReadAsStringAsync().Result;

					_logger.LogInformation($"WorkController(393): Result: {result}");
				}
			}

			var msg = new MessageBuilder()
				.KButton("⬅️назад")
				.SetChatId(userId)
				.PushL($"Спасибо за вашу выполненную работу\n\nВаш новый баланс: {user.Balance + price}грн. /{(user.Balance + price) * 1.93}руб.\n\nВы можете вернуться в главное меню и заработать больше денег");

			await _sender.Send(msg);
		}
		else if(accept == "no")
		{
			PushL($"Укажите причину отказа задания: /reason {userId} [текст причины до 10 слов]");
			await Send();
		}
		else
		{
			throw new Exception("CallbackAcceptWorkHandler: accept not set");
		}
	}

	[Action("/reason")]
	public async Task Reason(long userId, string reason)
	{
		var msg = new MessageBuilder()
				.KButton("⬅️назад")
				.SetChatId(userId)
				.PushL($"Вы не выполнили задание до конца, поэтому мы не можем оплатить вам задание.\n\nПричина: {reason}\n\nНачните выполнять задание заново и выполните его до конца либо выберите другое");

		await _sender.Send(msg);

		msg = new MessageBuilder()
			.SetChatId(long.Parse(_configuration["baseId"]))
			.PushL("Ответ отправлен");

		await _sender.Send(msg);
	}

	[Action("/reason")]
	public async Task Reason(long userId, string reason, string reason1)
	{
		var msg = new MessageBuilder()
				.KButton("⬅️назад")
				.SetChatId(userId)
				.PushL($"Вы не выполнили задание до конца, поэтому мы не можем оплатить вам задание.\n\nПричина: {reason} {reason1}\n\nНачните выполнять задание заново и выполните его до конца либо выберите другое");

		await _sender.Send(msg);

		msg = new MessageBuilder()
			.SetChatId(long.Parse(_configuration["baseId"]))
			.PushL("Ответ отправлен");

		await _sender.Send(msg);
	}

	[Action("/reason")]
	public async Task Reason(long userId, string reason, string reason1, string reason2)
	{
		var msg = new MessageBuilder()
				.KButton("⬅️назад")
				.SetChatId(userId)
				.PushL($"Вы не выполнили задание до конца, поэтому мы не можем оплатить вам задание.\n\nПричина: {reason} {reason1} {reason2}\n\nНачните выполнять задание заново и выполните его до конца либо выберите другое");

		await _sender.Send(msg);

		msg = new MessageBuilder()
			.SetChatId(long.Parse(_configuration["baseId"]))
			.PushL("Ответ отправлен");

		await _sender.Send(msg);
	}

	[Action("/reason")]
	public async Task Reason(long userId, string reason, string reason1, string reason2, string reason3)
	{
		var msg = new MessageBuilder()
				.KButton("⬅️назад")
				.SetChatId(userId)
				.PushL($"Вы не выполнили задание до конца, поэтому мы не можем оплатить вам задание.\n\nПричина: {reason} {reason1} {reason2} {reason3}\n\nНачните выполнять задание заново и выполните его до конца либо выберите другое");

		await _sender.Send(msg);

		msg = new MessageBuilder()
			.SetChatId(long.Parse(_configuration["baseId"]))
			.PushL("Ответ отправлен");

		await _sender.Send(msg);
	}

	[Action("/reason")]
	public async Task Reason(long userId, string reason, string reason1, string reason2, string reason3, string reason4)
	{
		var msg = new MessageBuilder()
				.KButton("⬅️назад")
				.SetChatId(userId)
				.PushL($"Вы не выполнили задание до конца, поэтому мы не можем оплатить вам задание.\n\nПричина: {reason} {reason1} {reason2} {reason3} {reason4}\n\nНачните выполнять задание заново и выполните его до конца либо выберите другое");

		await _sender.Send(msg);

		msg = new MessageBuilder()
			.SetChatId(long.Parse(_configuration["baseId"]))
			.PushL("Ответ отправлен");

		await _sender.Send(msg);
	}

	[Action("/reason")]
	public async Task Reason(long userId, string reason, string reason1, string reason2, string reason3, string reason4, string reason5)
	{
		var msg = new MessageBuilder()
				.KButton("⬅️назад")
				.SetChatId(userId)
				.PushL($"Вы не выполнили задание до конца, поэтому мы не можем оплатить вам задание.\n\nПричина: {reason} {reason1} {reason2} {reason3} {reason4} {reason5}\n\nНачните выполнять задание заново и выполните его до конца либо выберите другое");

		await _sender.Send(msg);

		msg = new MessageBuilder()
			.SetChatId(long.Parse(_configuration["baseId"]))
			.PushL("Ответ отправлен");

		await _sender.Send(msg);
	}

	[Action("/reason")]
	public async Task Reason(long userId, string reason, string reason1, string reason2, string reason3, string reason4, string reason5, string reason6)
	{
		var msg = new MessageBuilder()
				.KButton("⬅️назад")
				.SetChatId(userId)
				.PushL($"Вы не выполнили задание до конца, поэтому мы не можем оплатить вам задание.\n\nПричина: {reason} {reason1} {reason2} {reason3} {reason4} {reason5} {reason6}\n\nНачните выполнять задание заново и выполните его до конца либо выберите другое");

		await _sender.Send(msg);

		msg = new MessageBuilder()
			.SetChatId(long.Parse(_configuration["baseId"]))
			.PushL("Ответ отправлен");

		await _sender.Send(msg);
	}

	[Action("/reason")]
	public async Task Reason(long userId, string reason, string reason1, string reason2, string reason3, string reason4, string reason5, string reason6, string reason7)
	{
		var msg = new MessageBuilder()
				.KButton("⬅️назад")
				.SetChatId(userId)
				.PushL($"Вы не выполнили задание до конца, поэтому мы не можем оплатить вам задание.\n\nПричина: {reason} {reason1} {reason2} {reason3} {reason4} {reason5} {reason6} {reason7}\n\nНачните выполнять задание заново и выполните его до конца либо выберите другое");

		await _sender.Send(msg);

		msg = new MessageBuilder()
			.SetChatId(long.Parse(_configuration["baseId"]))
			.PushL("Ответ отправлен");

		await _sender.Send(msg);
	}

	[Action("/reason")]
	public async Task Reason(long userId, string reason, string reason1, string reason2, string reason3, string reason4, string reason5, string reason6, string reason7, string reason8)
	{
		var msg = new MessageBuilder()
				.KButton("⬅️назад")
				.SetChatId(userId)
				.PushL($"Вы не выполнили задание до конца, поэтому мы не можем оплатить вам задание.\n\nПричина: {reason} {reason1} {reason2} {reason3} {reason4} {reason5} {reason6} {reason7} {reason8}\n\nНачните выполнять задание заново и выполните его до конца либо выберите другое");

		await _sender.Send(msg);

		msg = new MessageBuilder()
			.SetChatId(long.Parse(_configuration["baseId"]))
			.PushL("Ответ отправлен");

		await _sender.Send(msg);
	}

	[Action("/reason")]
	public async Task Reason(long userId, string reason, string reason1, string reason2, string reason3, string reason4, string reason5, string reason6, string reason7, string reason8, string reason9)
	{
		var msg = new MessageBuilder()
				.KButton("⬅️назад")
				.SetChatId(userId)
				.PushL($"Вы не выполнили задание до конца, поэтому мы не можем оплатить вам задание.\n\nПричина: {reason} {reason1} {reason2} {reason3} {reason4} {reason5} {reason6} {reason7} {reason8} {reason9}\n\nНачните выполнять задание заново и выполните его до конца либо выберите другое");

		await _sender.Send(msg);

		msg = new MessageBuilder()
			.SetChatId(long.Parse(_configuration["baseId"]))
			.PushL("Ответ отправлен");

		await _sender.Send(msg);
	}
}









