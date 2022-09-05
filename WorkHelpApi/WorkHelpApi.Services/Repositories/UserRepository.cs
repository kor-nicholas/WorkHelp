using WorkHelpApi.Models;
using WorkHelpApi.Models.Entities;
using WorkHelpApi.Models.Input;

using WorkHelpApi.Abstractions;
using WorkHelpApi.Abstractions.Repositories;
using WorkHelpApi.Abstractions.Services;

using WorkHelpApi.Core;

using System.Collections.Generic;

namespace WorkHelpApi.Services.Repositories;

public class UserRepository : IUserRepository
{
	private MsSqlDbContext _dbContext;
	private ICommonService _commonService;

	public UserRepository(MsSqlDbContext dbContext, ICommonService commonService)
	{
		_dbContext = dbContext;
		_commonService = commonService;
	}

	public async Task<List<UserEntity>> GetAllUsers()
	{
		return _dbContext.Users.ToList();
	}

	public async Task<UserEntity> GetUserByUserId(string userId)
	{
		var user = _dbContext.Users.Single(user => user.UserIdTelegram == userId);

		if(user != null)
		{
			return user;
		}
		else
		{
			throw new Exception("Error in UserRepository at 35 line and user is null");
		}
	}

	public async Task<BoolModelWithErrorMessage> AddUser(UserInput userInput)
	{
		try
		{
			_dbContext.Users.Add(new UserEntity {
					Name = userInput.Name,
					Surname = userInput.Surname,
					Phone = userInput.Phone,
					Email = userInput.Email,
					UserIdTelegram = userInput.UserIdTelegram,
					NicknameTelegram = userInput.NicknameTelegram,
					Login = userInput.Login,
					Pass = userInput.Pass,
					Role = userInput.Role,
					Balance = userInput.Balance,
					Salt = userInput.Salt,
					RefferUserIdTelegram = userInput.RefferUserIdTelegram,
					RefferNicknameTelegram = userInput.RefferNicknameTelegram,
					CompletedTaskCount = userInput.CompletedTaskCount,
					RefferalsCount = userInput.RefferalsCount}
				);
			
			_dbContext.SaveChanges();

			var user = _dbContext.Users.Single(user => user.Name == userInput.Name);
			user.Pass = await _commonService.Sha256HashPass(userInput.Name, userInput.Pass);

			_dbContext.SaveChanges();

			return new BoolModelWithErrorMessage { IsError = false };
		}
		catch (Exception e)
		{
			return new BoolModelWithErrorMessage { IsError = true, ErrorMessage = e.Message };
		}
	}

	public async Task<BoolModelWithErrorMessage> AddRefferData(RefferalInput refferalInput)
	{
		try
		{
			var referal = _dbContext.Users.Single(user => user.UserIdTelegram == refferalInput.RefferalUserIdTelegram);
			var reffer = _dbContext.Users.Single(user => user.UserIdTelegram == refferalInput.RefferUserIdTelegram);

			referal.RefferUserIdTelegram = refferalInput.RefferUserIdTelegram;
			referal.RefferNicknameTelegram = refferalInput.RefferNicknameTelegram;
			reffer.RefferalsCount = reffer.RefferalsCount + 1;

			_dbContext.SaveChanges();

			return new BoolModelWithErrorMessage { IsError = false };
		}
		catch(Exception e)
		{
			return new BoolModelWithErrorMessage { IsError = true, ErrorMessage = e.Message };
		}
	}

	public async Task<BoolModelWithErrorMessage> DeleteUserById(int id)
	{
		try
		{
			var user = _dbContext.Users.Single(user => user.Id == id);
			_dbContext.Remove(user);

			_dbContext.SaveChanges();

			return new BoolModelWithErrorMessage { IsError = false };
		}
		catch (Exception e)
		{
			return new BoolModelWithErrorMessage { IsError = true, ErrorMessage = e.Message };
		}
	}

	public async Task<BoolModelWithErrorMessage> ClearUsers()
	{
		try
		{
			foreach (var item in _dbContext.Users)
			{
				_dbContext.Users.Remove(item);
			}

			_dbContext.SaveChanges();

			return new BoolModelWithErrorMessage { IsError = false };
		}
		catch (Exception e)
		{
			return new BoolModelWithErrorMessage { IsError = true, ErrorMessage = e.Message };
		}
	}

	public async Task<BoolModelWithErrorMessage> UpdateUser(UserInput userInput)
	{
		try
		{
			var user = _dbContext.Users.Single(user => user.Name == userInput.Name);

			user.Surname = userInput.Surname;
			user.Phone = userInput.Phone;
			user.Email = userInput.Email;
			user.UserIdTelegram = userInput.UserIdTelegram;
			user.NicknameTelegram = userInput.NicknameTelegram;
			user.Pass = await _commonService.Sha256HashPass(userInput.Name, userInput.Pass);
			user.Login = userInput.Login;
			user.Role = userInput.Role;
			user.Balance = userInput.Balance;
			user.Salt = userInput.Salt;
			user.RefferUserIdTelegram = userInput.RefferUserIdTelegram;
			user.RefferNicknameTelegram = userInput.RefferNicknameTelegram;
			user.CompletedTaskCount = userInput.CompletedTaskCount;
			user.RefferalsCount = userInput.RefferalsCount;

			_dbContext.SaveChanges();

			return new BoolModelWithErrorMessage { IsError = false };
		}
		catch (Exception e)
		{
			return new BoolModelWithErrorMessage { IsError = true, ErrorMessage = e.Message };
		}
	}

	public async Task<BoolModelWithErrorMessage> UpdateBalance(string userId, int balance)
	{
		try
		{
			var user = _dbContext.Users.Single(user => user.UserIdTelegram == userId);

			user.Balance += balance;

			_dbContext.SaveChanges();

			return new BoolModelWithErrorMessage { IsError = false };
		}
		catch (Exception e)
		{
			return new BoolModelWithErrorMessage { IsError = true, ErrorMessage = e.Message };
		}

	}
}



