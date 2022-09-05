using Deployf.Botf;
using Newtonsoft.Json;

using WorkHelpBot.Models.Output;

namespace WorkHelpBot.Services;

public class UserService : IBotUserService
{
	readonly IConfiguration _configuration;
	readonly ILogger<UserService> _logger;

	public UserService(IConfiguration configuration, ILogger<UserService> logger)
	{
		_configuration = configuration;
		_logger = logger;
	}

	public ValueTask<(string? id, string[]? roles)> GetUserIdWithRoles(long userId)
	{
		UserOutput user = null;

		using(var client = new HttpClient())
		{
			try
			{
				var endPoint = new Uri($"{_configuration["apiUrl"]}/user/get/{userId}");
				var json = client.GetAsync(endPoint).Result.Content.ReadAsStringAsync().Result;
				user = JsonConvert.DeserializeObject<UserOutput>(json);

				_logger.LogInformation($"UserService(18): /user/get Result: {user}");

				var roles = new string[1];
				roles[0] = user.Role;

				return new (($"{userId}", roles));
			}
			catch(Newtonsoft.Json.JsonReaderException)
			{
				return new ((null, null));
			}
		}
	}
}




