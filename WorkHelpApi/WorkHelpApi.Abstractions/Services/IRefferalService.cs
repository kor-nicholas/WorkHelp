using WorkHelpApi.Models.Output;
using WorkHelpApi.Models.Input;
using WorkHelpApi.Models;

namespace WorkHelpApi.Abstractions.Services;

public interface IRefferalService
{
	Task<List<RefferalOutput>> GetRefferalsByUserId(string userId);
	Task<BoolModelWithErrorMessage> AddRefferal(RefferalInput refferalInput);
	Task<BoolModelWithErrorMessage> UpdateRefferal(RefferalInput refferalInput);
}
