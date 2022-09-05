using WorkHelpApi.Abstractions.Repositories;

using WorkHelpApi.Core;

using WorkHelpApi.Models;
using WorkHelpApi.Models.Entities;

namespace WorkHelpApi.Services.Repositories;

public class PromocodeRepository : IPromocodeRepository
{
	private MsSqlDbContext _dbContext;

	public PromocodeRepository(MsSqlDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<BoolModelWithErrorMessage> UpdatePromocode(string name, string userId)
	{
		var promocode = _dbContext.Promocodes.Single(promocode => promocode.Name == name);

		if(promocode != null)
		{
			promocode.ActivatedUserId += $"{userId};";
			_dbContext.SaveChanges();

			return new BoolModelWithErrorMessage { IsError = false };
		}
		else
		{
			return new BoolModelWithErrorMessage { IsError = true, ErrorMessage = "PromocodeRepository(23): promocode is null" };
		}
	}

	public async Task<BoolModelWithErrorMessage> AddPromocode(string name, DateTime endDate, int bonus)
	{
		try
		{
			_dbContext.Add(new PromocodeEntity { Name = name, EndDate = endDate, Bonus = bonus, ActivatedUserId = "" });
			_dbContext.SaveChanges();

			return new BoolModelWithErrorMessage { IsError = false };
		}
		catch(Exception e)
		{
			return new BoolModelWithErrorMessage { IsError = true, ErrorMessage = e.Message };
		}
	}

	public async Task<BoolModelWithErrorMessage> DeletePromocode(string name)
	{
		try
		{
			var promocode = _dbContext.Promocodes.Single(promocode => promocode.Name == name);
			_dbContext.Remove(promocode);

			_dbContext.SaveChanges();

			return new BoolModelWithErrorMessage { IsError = false };
		}
		catch(Exception e)
		{
			return new BoolModelWithErrorMessage { IsError = true, ErrorMessage = e.Message };
		}
	}

	public async Task<List<PromocodeEntity>> GetPromocodes()
	{
		return _dbContext.Promocodes.ToList();
	}

	public async Task<PromocodeEntity> GetPromocodeForName(string name)
	{
		var promocode = _dbContext.Promocodes.Single(promocode => promocode.Name == name);

		if(promocode != null)
		{
			return promocode;
		}
		else
		{
			throw new NullReferenceException("PromocodeRepository(75): promocode is null");
		}
	}
}






