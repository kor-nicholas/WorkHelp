using WorkHelpApi.Models;
using WorkHelpApi.Models.Entities;
using WorkHelpApi.Models.Input;

using WorkHelpApi.Abstractions;
using WorkHelpApi.Abstractions.Repositories;
using WorkHelpApi.Abstractions.Services;

using WorkHelpApi.Core;

using System.Collections.Generic;

namespace WorkHelpApi.Services.Repositories;

public class RefferalRepository : IRefferalRepository
{
	private MsSqlDbContext _dbContext;

	public RefferalRepository(MsSqlDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<List<RefferalEntity>> GetRefferalsByUserId(string userId)
	{
		return _dbContext.Refferals.Where(refferal => refferal.RefferUserIdTelegram == userId).ToList();
	}

	public async Task<BoolModelWithErrorMessage> AddRefferal(RefferalInput refferalInput)
	{
		try
		{
			_dbContext.Refferals.Add(new RefferalEntity {
					RefferalUserIdTelegram = refferalInput.RefferalUserIdTelegram,
					RefferalNicknameTelegram = refferalInput.RefferalNicknameTelegram,
					RefferUserIdTelegram = refferalInput.RefferUserIdTelegram,
					RefferNicknameTelegram = refferalInput.RefferNicknameTelegram,
					Earned = refferalInput.Earned}
				);
			
			_dbContext.SaveChanges();

			return new BoolModelWithErrorMessage { IsError = false };
		}
		catch (Exception e)
		{
			return new BoolModelWithErrorMessage { IsError = true, ErrorMessage = e.Message };
		}
	}

	public async Task<BoolModelWithErrorMessage> UpdateRefferal(RefferalInput refferalInput)
	{
		try
		{
			var refferal = _dbContext.Refferals.Single(refferal => refferal.RefferUserIdTelegram == refferalInput.RefferUserIdTelegram);

			refferal.RefferalUserIdTelegram = refferalInput.RefferalUserIdTelegram;
			refferal.RefferalNicknameTelegram = refferalInput.RefferalNicknameTelegram;
			refferal.RefferUserIdTelegram = refferalInput.RefferUserIdTelegram;
			refferal.RefferNicknameTelegram = refferalInput.RefferNicknameTelegram;
			refferal.Earned = refferalInput.Earned;

			_dbContext.SaveChanges();

			return new BoolModelWithErrorMessage { IsError = false };
		}
		catch (Exception e)
		{
			return new BoolModelWithErrorMessage { IsError = true, ErrorMessage = e.Message };
		}
	}
}



