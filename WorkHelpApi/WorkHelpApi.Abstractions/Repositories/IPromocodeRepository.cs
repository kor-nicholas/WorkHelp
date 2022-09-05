using WorkHelpApi.Models;
using WorkHelpApi.Models.Entities;

namespace WorkHelpApi.Abstractions.Repositories;

public interface IPromocodeRepository
{
	Task<BoolModelWithErrorMessage> UpdatePromocode(string name, string userId);
	Task<BoolModelWithErrorMessage> AddPromocode(string name, DateTime endDate, int bonus);
	Task<BoolModelWithErrorMessage> DeletePromocode(string name);
	Task<List<PromocodeEntity>> GetPromocodes();
	Task<PromocodeEntity> GetPromocodeForName(string name);
}

