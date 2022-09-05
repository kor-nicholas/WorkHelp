using WorkHelpApi.Models;
using WorkHelpApi.Models.Output;

namespace WorkHelpApi.Abstractions.Services;

public interface IPromocodeService
{
	Task<BoolModelWithErrorMessage> UpdatePromocode(string name, string userId);
	Task<BoolModelWithErrorMessage> AddPromocode(string name, DateTime endDate, int bonus);
	Task<BoolModelWithErrorMessage> DeletePromocode(string name);
	Task<List<PromocodeOutput>> GetPromocodes();
	Task<PromocodeOutput> GetPromocodeForName(string name);
}


