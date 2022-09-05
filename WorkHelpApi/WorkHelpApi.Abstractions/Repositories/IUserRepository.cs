using WorkHelpApi.Models.Entities;
using WorkHelpApi.Models;
using WorkHelpApi.Models.Input;

namespace WorkHelpApi.Abstractions.Repositories;

public interface IUserRepository
{
	Task<List<UserEntity>> GetAllUsers();
	Task<UserEntity> GetUserByUserId(string userId);
	Task<BoolModelWithErrorMessage> AddUser(UserInput userInput);
	Task<BoolModelWithErrorMessage> AddRefferData(RefferalInput refferalInput);
	Task<BoolModelWithErrorMessage> DeleteUserById(int id);
	Task<BoolModelWithErrorMessage> ClearUsers();
	Task<BoolModelWithErrorMessage> UpdateUser(UserInput userInput);
	Task<BoolModelWithErrorMessage> UpdateBalance(string userId, int balance);
}
