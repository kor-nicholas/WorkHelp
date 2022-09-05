using WorkHelpApi.Models.Entities;
using WorkHelpApi.Models;
using WorkHelpApi.Models.Input;

namespace WorkHelpApi.Abstractions.Repositories;

public interface IRefferalRepository
{
	Task<List<RefferalEntity>> GetRefferalsByUserId(string userId);
	Task<BoolModelWithErrorMessage> AddRefferal(RefferalInput refferalInput);
	Task<BoolModelWithErrorMessage> UpdateRefferal(RefferalInput refferalInput);
}
