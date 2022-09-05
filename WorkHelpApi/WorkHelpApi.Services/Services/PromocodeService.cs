using WorkHelpApi.Models;
using WorkHelpApi.Models.Output;

using WorkHelpApi.Abstractions.Services;
using WorkHelpApi.Abstractions.Repositories;

namespace WorkHelpApi.Services.Services;

public class PromocodeService : IPromocodeService
{
	readonly IPromocodeRepository _promocodeRepository;
	readonly ICommonService _commonService;

	public PromocodeService(IPromocodeRepository promocodeRepository, ICommonService commonService)
	{
		_promocodeRepository = promocodeRepository;
		_commonService = commonService;
	}

	public async Task<BoolModelWithErrorMessage> UpdatePromocode(string name, string userId)
	{
		return await _promocodeRepository.UpdatePromocode(name, userId);
	}

	public async Task<BoolModelWithErrorMessage> AddPromocode(string name, DateTime endDate, int bonus)
	{
		return await _promocodeRepository.AddPromocode(name, endDate, bonus);
	}

	public async Task<BoolModelWithErrorMessage> DeletePromocode(string name)
	{
		return await _promocodeRepository.DeletePromocode(name);
	}

	public async Task<List<PromocodeOutput>> GetPromocodes()
	{
		var result = await _promocodeRepository.GetPromocodes();
		return await _commonService.MapPromocodeEntitiesToPromocodeOutput(result);
	}

	public async Task<PromocodeOutput> GetPromocodeForName(string name)
	{
		var result = await _promocodeRepository.GetPromocodeForName(name);
		return await _commonService.MapPromocodeEntityToPromocodeOutput(result);
	}
}




