using Deployf.Botf;

public class ManualController : BotController
{
	[Action("📜мануалы📜")]
	public async Task Manuals()
	{
		PushL("Пробный урок в Skyeng\n\nhttps://telegra.ph/Manual-po-inglishu-Skyengru-08-07");
		await Send();
	}
}





