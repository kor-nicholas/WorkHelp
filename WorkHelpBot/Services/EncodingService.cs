using Deployf.Botf;

using WorkHelpBot.Interfaces;

namespace WorkHelpBot.Services;

public class EncodingService : IEncodingService
{
	readonly ILogger<EncodingService> _logger;

	public EncodingService(ILogger<EncodingService> logger)
	{
		_logger = logger;
	}

	public async Task<string> Encode(string msg)
	{
		byte[] bytes = System.Text.Encoding.GetEncoding(12000).GetBytes(msg);

		return Convert.ToBase64String(bytes);
	}

	public async Task<string> Decode(string encodedMsg)
	{
		byte[] bytes = Convert.FromBase64String(encodedMsg);

		return System.Text.Encoding.GetEncoding(12000).GetString(bytes);
	}
}



