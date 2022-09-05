using Deployf.Botf;

class SupportController : BotController
{
	[Action("✅служба поддержки✅")]
	public async Task Support()
	{
		Button("Написать менеджеру", "https://t.me/kornic145");
		PushL("Менеджеры служби поддержки");
		await Send();

		KButton("⬅️назад");
		PushL("Если вы хотите вернуться в главное меню -> Нажмите на кнопку 'Назад'");
		await Send();
	}
}
