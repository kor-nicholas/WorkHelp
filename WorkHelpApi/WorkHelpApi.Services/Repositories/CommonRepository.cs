using WorkHelpApi.Models;
using WorkHelpApi.Abstractions.Repositories;

using WorkHelpApi.Core;

namespace WorkHelpApi.Services.Repositories;

public class CommonRepository : ICommonRepository
{
	private MsSqlDbContext _dbContext;

	public CommonRepository(MsSqlDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<BoolModelWithErrorMessage> AddSaltForId(string name, string salt)
	{
		try
		{
			var user = _dbContext.Users.Single(user => user.Name == name);

			user.Salt = salt;

			return new BoolModelWithErrorMessage { IsError = false };
		}
		catch(Exception e)
		{
			return new BoolModelWithErrorMessage { IsError = true, ErrorMessage = e.Message };
		}
	}
}


