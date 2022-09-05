using Deployf.Botf;
using Newtonsoft.Json;

using WorkHelpBot.Models.Output;
using WorkHelpBot.Models.Input;

class WithdrawalController : BotController
{
	readonly IConfiguration _configuration;
	readonly ILogger<WithdrawalController> _logger;

	public WithdrawalController(IConfiguration configuration, ILogger<WithdrawalController> logger)
	{
		_configuration = configuration;
		_logger = logger;
	}

	[Action("💳вывод💳")]
	public async Task Withdrawal()
	{
		UserOutput user = null;

		using(var client = new HttpClient())
		{
			var endPoint = new Uri($"{_configuration["apiUrl"]}/user/get/{FromId}");
			var json = client.GetAsync(endPoint).Result.Content.ReadAsStringAsync().Result;
			user = JsonConvert.DeserializeObject<UserOutput>(json);

			_logger.LogInformation($"WithdrawalController(26): Result: {user}");
		}

		if(user != null)
		{
			if(user.Balance < 100)
			{
				KButton("⬅️назад");
				PushL("Минимальная сумма вывода - 100грн. / 193руб. Выполняйте задания(в раздале '💸заработок💸'), приглашайте рефералов либо ищите промокоды у нас в соц.сетях, чтобы набрать минимальную сумму и повторите попытку");
				await Send();
			}
			else if(user.CompletedTaskCount < 2)
			{
				KButton("⬅️назад");
				PushL("Для вывода нужно выполнить минимум 2 задания. Выполните 2 задания(в раздале '💸заработок💸') и повторите попытку");
				await Send();
			}
			else if(user.RefferalsCount < 3)
			{
				KButton("⬅️назад");
				PushL("Для вывода нужно пригласить минимум 3 рефералов. Пригласите 3 рефералов и повторите попытку");
				await Send();
			}
			else
			{
				KButton("ПриватБанк");
				KButton("МоноБанк");
				RowKButton("Visa/Mastercard");
				RowKButton("Qiwi");
				KButton("ЮMoney");
				RowKButton("Tinkoff");
				KButton("Мир");
				KButton("СберБанк");
				RowKButton("⬅️назад");
				PushL($"Твой баланс: {user.Balance}\n\nМинимальная сумма вывода - 100грн. / 193руб.\nМаксимальная сумма вывода - 10000грн. / 19300руб.\n\nВыберите способ вывода");
				await Send();
			}

		}
		else
		{
			KButton("⬅️назад");
			PushL("Приносим извинения. Произошла ошибка. Попробуйте еще раз");
			await Send();

			throw new NullReferenceException("WithdrawalController(32): user is null");
		}
	}

	[Action("ПриватБанк")]
	[Action("МоноБанк")]
	[Action("Visa/Mastercard")]
	[Action("Tinkoff")]
	[Action("Мир")]
	[Action("СберБанк")]
	public async Task BankWithdrawal()
	{
		PushL("Введите номер карты: ");
		await Send();
		double cardNumber = 0;

		try
		{
			cardNumber = double.Parse(await AwaitText());
		}
		catch(FormatException)
		{
			Button("Повторить попытку");
			PushL("Номер карты должен быть числом");
			await Send();

			var callCardNumber = await AwaitQuery();

			if(callCardNumber == "Повторить попытку")
			{
				await BankWithdrawal();
				return;
			}
		}

		PushL("Введите сумму вывода(в грн): ");
		await Send();
		int sum = 0;

		try
		{
			sum = int.Parse(await AwaitText());
		}
		catch(FormatException)
		{
			Button("Повторить попытку");
			PushL("Сума должна быть числом");
			await Send();

			var callSum = await AwaitQuery();

			if(callSum == "Повторить попытку")
			{
				await BankWithdrawal();
				return;
			}
		}
		
		using(var client = new HttpClient())
		{
			var endPoint = new Uri($"{_configuration["apiUrl"]}/user/get/{FromId}");
			var json = client.GetAsync(endPoint).Result.Content.ReadAsStringAsync().Result;
			UserOutput user = JsonConvert.DeserializeObject<UserOutput>(json);

			_logger.LogInformation($"WithdrawalController(64): Result: {user}");

			if(sum > user.Balance)
			{
				RowKButton("⬅️назад");
				PushL("На балансе не достаточно средств");
				await Send();
			}
			else
			{
				using(var client2 = new HttpClient())
				{
					var endPoint2 = new Uri($"{_configuration["apiUrl"]}/user/update");
					var jsonUser = JsonConvert.SerializeObject(new UserInput { Id = user.Id,
																	Name = user.Name,
																	Surname = user.Surname,
																	Phone = user.Phone,
																	Email = user.Email,
																	UserIdTelegram = user.UserIdTelegram,
																	NicknameTelegram = user.NicknameTelegram,
																	Pass = user.Pass,
																	Login = user.Login,
																	Role = user.Role,
																	Balance = user.Balance - sum,
																	Salt = user.Salt,
																	RefferUserIdTelegram = user.RefferUserIdTelegram,
																	RefferNicknameTelegram = user.RefferNicknameTelegram,
																	CompletedTaskCount = user.CompletedTaskCount,
																	RefferalsCount = user.RefferalsCount});
					var payload = new StringContent(jsonUser, System.Text.Encoding.UTF8, "application/json");
					var result = client2.PutAsync(endPoint2, payload).Result.Content.ReadAsStringAsync().Result;

					_logger.LogInformation($"Result: {result}");
				}

				RowKButton("⬅️назад");
				PushL("Благодарим за работу с нами. Ваша заявка отправлена на модерацию. Сейчас можете вернуться в главное меню и выполнить еще больше заданий");
				await Send();
			}
		}
	}

	[Action("Qiwi")]
	[Action("ЮMoney")]
	public async Task BillWithdrawal()
	{
		PushL("Введите номер телефона/счета для вывода: ");
		await Send();
		var billNumber = await AwaitText();

		PushL("Введите сумму вывода(в грн): ");
		await Send();
		int sum = 0;

		try
		{
			sum = int.Parse(await AwaitText());
		}
		catch(FormatException)
		{
			Button("Повторить попытку");
			PushL("Сума должна быть числом");
			await Send();

			var callSum = await AwaitQuery();

			if(callSum == "Повторить попытку")
			{
				await BankWithdrawal();
				return;
			}
		}

		using(var client = new HttpClient())
		{
			var endPoint = new Uri($"{_configuration["apiUrl"]}/user/get/{FromId}");
			var json = client.GetAsync(endPoint).Result.Content.ReadAsStringAsync().Result;
			UserOutput user = JsonConvert.DeserializeObject<UserOutput>(json);

			if(sum > user.Balance)
			{
				RowKButton("⬅️назад");
				PushL("На балансе не достаточно средств");
				await Send();
			}
			else
			{
				using(var client2 = new HttpClient())
				{
					var endPoint2 = new Uri($"{_configuration["apiUrl"]}/user/update");
					var jsonUser = JsonConvert.SerializeObject(new UserInput { Id = user.Id,
																	Name = user.Name,
																	Surname = user.Surname,
																	Phone = user.Phone,
																	Email = user.Email,
																	UserIdTelegram = user.UserIdTelegram,
																	NicknameTelegram = user.NicknameTelegram,
																	Pass = user.Pass,
																	Login = user.Login,
																	Role = user.Role,
																	Balance = user.Balance - sum,
																	Salt = user.Salt,
																	RefferUserIdTelegram = user.RefferUserIdTelegram,
																	RefferNicknameTelegram = user.RefferNicknameTelegram,
																	CompletedTaskCount = user.CompletedTaskCount,
																	RefferalsCount = user.RefferalsCount});
					var payload = new StringContent(jsonUser, System.Text.Encoding.UTF8, "application/json");
					var result = client2.PutAsync(endPoint2, payload).Result.Content.ReadAsStringAsync().Result;

					_logger.LogInformation($"Result: {result}");
				}

				RowKButton("⬅️назад");
				PushL("Благодарим за работу с нами. Ваша заявка отправлена на модерацию. Сейчас можете вернуться в главное меню и выполнить еще больше заданий");
				await Send();
			}
		}
	}
}
