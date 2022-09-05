using Deployf.Botf;
using Newtonsoft.Json;

using WorkHelpBot.Models.Output;

using WorkHelpBot.Interfaces;

namespace WorkHelpBot.Controllers;

class ProfileController : BotController
{
	readonly IConfiguration _configuration;
	readonly IEncodingService _encodingService;

	public ProfileController(IConfiguration configuration, IEncodingService encodingService)
	{
		_configuration = configuration;
		_encodingService = encodingService;
	}

	[Action("👤профиль👤")]
	public async Task Profile()
	{
		UserOutput user = null;

		using(var client = new HttpClient())
		{
			var endPoint = new Uri($"{_configuration["apiUrl"]}/user/get/{FromId}");
			var json = client.GetAsync(endPoint).Result.Content.ReadAsStringAsync().Result;
			user = JsonConvert.DeserializeObject<UserOutput>(json);
		}

		KButton("⬅️назад");
		PushL($"Привет, {user.Login}\n\nИмя и фамилия: {await _encodingService.Decode(user.Name)} {await _encodingService.Decode(user.Surname)} @{user.NicknameTelegram}\n\nТелефон: {user.Phone}\nEmail: {user.Email}\n\nБаланс: {user.Balance}грн./{user.Balance * 1.93}руб.");
		await Send();
	}
}




