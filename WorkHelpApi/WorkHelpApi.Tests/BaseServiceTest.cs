using WorkHelpApi.Core;
using WorkHelpApi.Services.Services;
using WorkHelpApi.Services.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System.IO;

namespace WorkHelpApi.Tests;

public class BaseServiceTest
{
	protected string ConnectionString { get; set; }
	protected IConfiguration Configuration { get; set; }
	protected MsSqlDbContext MsSqlDbContext { get; set; }

	protected CommonService CommonService { get; set; }
	protected CommonRepository CommonRepository { get; set; }

	public BaseServiceTest()
	{
		var builder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.AddEnvironmentVariables();

		Configuration = builder.Build();

		ConnectionString = Configuration.GetConnectionString("MsSqlApiConnection");

		var optionsBuilder = new DbContextOptionsBuilder<MsSqlDbContext>();
		optionsBuilder.UseSqlServer(ConnectionString);
		MsSqlDbContext = new MsSqlDbContext(optionsBuilder.Options);

		CommonRepository = new CommonRepository(MsSqlDbContext);
		CommonService = new CommonService(CommonRepository);
	}
}



