using WorkHelpApi.Models;
using WorkHelpApi.Models.Input;
using WorkHelpApi.Models.Output;
using WorkHelpApi.Models.Entities;

using WorkHelpApi.Abstractions.Services;
using WorkHelpApi.Abstractions.Repositories;

namespace WorkHelpApi.Services.Services;

public class UserService : IUserService
{
	private IUserRepository _userRepository;
	private ICommonService _commonService;

	public UserService(IUserRepository userRepository, ICommonService commonService)
	{
		_userRepository = userRepository;
		_commonService = commonService;
	}

	public async Task<List<UserOutput>> GetAllUsers()
	{
		var result = await _userRepository.GetAllUsers();
		return await _commonService.MapUserEntitiesToUserOutput(result);
	}

	public async Task<UserOutput> GetUserByUserId(string userId)
	{
		var result = await _userRepository.GetUserByUserId(userId);
		return await _commonService.MapUserEntityToUserOutput(result);
	}

	public async Task<BoolModelWithErrorMessage> AddUser(UserInput userInput)
	{
		return await _userRepository.AddUser(userInput);
	}

	public async Task<BoolModelWithErrorMessage> AddRefferData(RefferalInput refferalInput)
	{
		return await _userRepository.AddRefferData(refferalInput);
	}

	public async Task<BoolModelWithErrorMessage> DeleteUserById(int id)
	{
		return await _userRepository.DeleteUserById(id);
	}

	public async Task<BoolModelWithErrorMessage> ClearUsers()
	{
		return await _userRepository.ClearUsers();
	}

	public async Task<BoolModelWithErrorMessage> UpdateUser(UserInput userInput)
	{
		return await _userRepository.UpdateUser(userInput);
	}

	public async Task<BoolModelWithErrorMessage> UpdateBalance(string userId, int balance)
	{
		return await _userRepository.UpdateBalance(userId, balance);
	}
}
