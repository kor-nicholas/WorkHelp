using WorkHelpApi.Models.Output;
using WorkHelpApi.Models.Input;
using WorkHelpApi.Models;

namespace WorkHelpApi.Abstractions.Services;

public interface IUserService
{
	Task<List<UserOutput>> GetAllUsers();
	Task<UserOutput> GetUserByUserId(string userId);
	Task<BoolModelWithErrorMessage> AddUser(UserInput userInput);
	Task<BoolModelWithErrorMessage> AddRefferData(RefferalInput refferalInput);
	Task<BoolModelWithErrorMessage> DeleteUserById(int id);
	Task<BoolModelWithErrorMessage> ClearUsers();
	Task<BoolModelWithErrorMessage> UpdateUser(UserInput userInput);
	Task<BoolModelWithErrorMessage> UpdateBalance(string userId, int balance);
}
