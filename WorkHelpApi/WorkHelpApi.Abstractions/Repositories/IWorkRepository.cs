using WorkHelpApi.Models.Entities;
using WorkHelpApi.Models;
using WorkHelpApi.Models.Input;

namespace WorkHelpApi.Abstractions.Repositories;

public interface IWorkRepository
{
	Task<List<WorkEntity>> GetAllWorks();
	Task<WorkEntity> GetWorkById(int id);
	Task<BoolModelWithErrorMessage> AddWork(WorkInput workInput);
	Task<BoolModelWithErrorMessage> DeleteWorkByName(string name);
	Task<BoolModelWithErrorMessage> ClearWorks();
	Task<BoolModelWithErrorMessage> UpdateWork(WorkInput workInput);
}
