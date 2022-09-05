

namespace WorkHelpBot.Interfaces;

public interface IEncodingService
{
	Task<string> Encode(string msg);
	Task<string> Decode(string encodedMsg);
}




