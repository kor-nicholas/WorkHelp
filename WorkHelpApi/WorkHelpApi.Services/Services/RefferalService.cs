using WorkHelpApi.Models;
using WorkHelpApi.Models.Input;
using WorkHelpApi.Models.Output;
using WorkHelpApi.Models.Entities;
using WorkHelpApi.Abstractions.Services;
using WorkHelpApi.Abstractions.Repositories;

namespace WorkHelpApi.Services.Services;

public class RefferalService : IRefferalService
{
	private IRefferalRepository _refferalRepository;
	private ICommonService _commonService;

	public RefferalService(IRefferalRepository refferalRepository, ICommonService commonService)
	{
		_refferalRepository = refferalRepository;
		_commonService = commonService;
	}

	public async Task<List<RefferalOutput>> GetRefferalsByUserId(string userId)
	{
		var result = await _refferalRepository.GetRefferalsByUserId(userId);
		return await _commonService.MapRefferalEntitiesToRefferalOutput(result);
	}

	public async Task<BoolModelWithErrorMessage> AddRefferal(RefferalInput refferalInput)
	{
		return await _refferalRepository.AddRefferal(refferalInput);
	}

	public async Task<BoolModelWithErrorMessage> UpdateRefferal(RefferalInput refferalInput)
	{
		return await _refferalRepository.UpdateRefferal(refferalInput);
	}
}
