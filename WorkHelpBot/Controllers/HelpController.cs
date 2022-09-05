using Deployf.Botf;

class HelpController : BotController
{
	[Action("❗️помощь❗️")]
	public async Task Help()
	{
		
		KButton("📜мануалы📜");
		RowKButton("✅служба поддержки✅");
		KButton("📊оставить отзыв📊");
		RowKButton("⬅️назад");
		PushL("Раздел 'Помощь'\n\n(чтобы просмотреть список команд введите команду /commands)");
		await Send();

	}
}










