using Deployf.Botf;

public class CommandsController : BotController
{
	[Authorize("admin")]
	[Action("/commands", "Просмотреть список команд")]
	public async Task CommandsForAdmin()
	{
		KButton("⬅️назад");
		PushL("Admin commands: \n\n/role [userId] [newRole] - set new role\n/add-promocode - add new promocode\n/delete-promocode - delete promocode\n/promocodes - check all promocodes\n/ping - ping bot to bot and api will not sleep\n/add-work - add new work\n/delete-work - delete work\n/update-work - update work\n/works - check all works");
		await Send();
	}

	[Authorize("user")]
	[Action("/commands", "Просмотреть список команд")]
	public async Task CommandsForUsers()
	{
		PushL("/sendredsurf - отправить данные redsurf-аккаунта, чтобы получить оплату за кредиты сайта\n/sendphotogame - отправить скриншот профиля браузерной игры для получения оплаты\n/sendstudy - получить оплату за вводный урок");
		await Send();
	}
}






