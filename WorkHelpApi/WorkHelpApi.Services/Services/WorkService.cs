using WorkHelpApi.Models;
using WorkHelpApi.Models.Input;
using WorkHelpApi.Models.Output;
using WorkHelpApi.Models.Entities;
using WorkHelpApi.Abstractions.Services;
using WorkHelpApi.Abstractions.Repositories;
using WorkHelpApi.Abstractions;

namespace WorkHelpApi.Services.Services;

public class WorkService : IWorkService
{
	private IWorkRepository _workRepository;
	private ICommonService _commonService;

	public WorkService(IWorkRepository workRepository, ICommonService commonService)
	{
		_workRepository = workRepository;
		_commonService = commonService;
	}

	public async Task<List<WorkOutput>> GetAllWorks()
	{
		var result = await _workRepository.GetAllWorks();
		return await _commonService.MapWorksEntityToWorkOutput(result);
	}
	
	public async Task<WorkOutput> GetWorkById(int id)
	{
		var result = await _workRepository.GetWorkById(id);

		return await _commonService.MapWorkEntityToWorkOutput(result);
	}

	public async Task<BoolModelWithErrorMessage> AddWork(WorkInput workInput)
	{
		return await _workRepository.AddWork(workInput);
	}

	public async Task<BoolModelWithErrorMessage> DeleteWorkByName(string name)
	{
		return await _workRepository.DeleteWorkByName(name);
	}

	public async Task<BoolModelWithErrorMessage> ClearWorks()
	{
		return await _workRepository.ClearWorks();
	}

	public async Task<BoolModelWithErrorMessage> UpdateWork(WorkInput workInput)
	{
		return await _workRepository.UpdateWork(workInput);
	}
}
