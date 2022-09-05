using Deployf.Botf;
using Newtonsoft.Json;

using WorkHelpBot.Models.Output;

public class RoleController : BotController
{
	readonly IConfiguration _configuration;
	readonly ILogger<RoleController> _logger;

	public RoleController(IConfiguration configuration, ILogger<RoleController> logger)
	{
		_configuration = configuration;
		_logger = logger;
	}

	[Authorize("admin")]
	[Action("/role")]
	public async Task Role(long userId, string role)
	{
		// TODO: this method can use only creator(I)

		UserOutput user = null;
		using (var client = new HttpClient())
		{
			var endPoint = new Uri($"{_configuration["apiUrl"]}/user/get/{userId}");
			var json = client.GetAsync(endPoint).Result.Content.ReadAsStringAsync().Result;
			user = JsonConvert.DeserializeObject<UserOutput>(json);

			_logger.LogInformation($"RoleController(25): /role: Result: {user}");
		}

		using(var client = new HttpClient())
		{
			var endPoint = new Uri($"{_configuration["apiUrl"]}/user/update");
			var jsonUser = JsonConvert.SerializeObject(new UserOutput { Name = user.Name, Surname = user.Surname, Phone = user.Phone, Email = user.Email, UserIdTelegram = user.UserIdTelegram, NicknameTelegram = user.NicknameTelegram, Pass = user.Pass, Login = user.Login, Role = role, Balance = user.Balance, Salt = user.Salt, RefferUserIdTelegram = user.RefferUserIdTelegram, RefferNicknameTelegram = user.RefferNicknameTelegram, CompletedTaskCount = user.CompletedTaskCount, RefferalsCount = user.RefferalsCount });
			var payload = new StringContent(jsonUser, System.Text.Encoding.UTF8, "application/json");
			var result = client.PutAsync(endPoint, payload).Result.Content.ReadAsStringAsync().Result;

			_logger.LogInformation($"RoleController(32): /role Result: {result}");
		}
	}
}




