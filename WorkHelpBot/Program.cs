using Deployf.Botf;

using WorkHelpBot.Services;
using WorkHelpBot.Interfaces;

BotfProgram.StartBot(args, onConfigure: (svc, cfg) =>
{
	svc.AddSingleton<IBotUserService, UserService>();
	svc.AddSingleton<IEncodingService, EncodingService>();
});
