using Deployf.Botf;

class BackConroller : BotController
{
	[Action("⬅️назад")]
	public async Task Back()
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
}
