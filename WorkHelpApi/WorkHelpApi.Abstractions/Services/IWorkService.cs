using WorkHelpApi.Models.Output;
using WorkHelpApi.Models.Input;
using WorkHelpApi.Models;

namespace WorkHelpApi.Abstractions.Services;

public interface IWorkService
{
	Task<List<WorkOutput>> GetAllWorks();
	Task<WorkOutput> GetWorkById(int id);
	Task<BoolModelWithErrorMessage> AddWork(WorkInput workInput);
	Task<BoolModelWithErrorMessage> DeleteWorkByName(string name);
	Task<BoolModelWithErrorMessage> ClearWorks();
	Task<BoolModelWithErrorMessage> UpdateWork(WorkInput workInput);
}
