using Deployf.Botf;
using Telegram.Bot;

class FeedbackController : BotController
{
	readonly IConfiguration _configuration;

	public FeedbackController(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	[Action("📊оставить отзыв📊")]
	public async Task WriteFeedback()
	{
		PushL("Напишите ваш отзыв: ");
		await Send();

		var feedback = await AwaitText();

		await Client.SendTextMessageAsync(_configuration["feedbackId"], $"Feedback from @{Context.GetUsername()}:\n{feedback}");

		KButton("⬅️назад");
		PushL("Спасибо, ваше мнение очень важно для нас. Пишите больше отзывов!😁❤️");
	}
}
