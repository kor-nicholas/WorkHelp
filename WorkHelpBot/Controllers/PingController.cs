using Deployf.Botf;
using System.Net;
using RandomUserAgent;

class PingController : BotController
{
	readonly ILogger _logger;
	private IConfiguration _configuration;

	public PingController(ILogger<PingController> logger, IConfiguration configuration)
	{
		_logger = logger;
		_configuration = configuration;
	}

	[Authorize("admin")]
	[Action("/ping")]
	public async Task Ping()
	{
		var url = "http://workhelpbot.somee.com";
		var rand = new Random();

		while(true)
		{
			try
			{
				using(var client = new HttpClient())
				{
					var endPoint = new Uri($"{url}");
					var json = await client.GetAsync(endPoint);

					_logger.LogInformation($"GET request completed ({url}) | {json}");

					endPoint = new Uri($"{_configuration["apiUrl"]}");
					json = await client.GetAsync(endPoint);

					_logger.LogInformation($"GET request completed ({_configuration["apiUrl"]}) | {json}");
				}
			}
			catch(Exception e)
			{
				_logger.LogCritical($"PingController: {e.Message}");
			}

			await Task.Delay(rand.Next(300000, 900000));
		}
	}
}
