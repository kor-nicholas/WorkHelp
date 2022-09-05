using System;
using System.Text;
using System.Security.Cryptography;

using WorkHelpApi.Abstractions.Services;
using WorkHelpApi.Abstractions.Repositories;

using WorkHelpApi.Models.Entities;
using WorkHelpApi.Models.Input;
using WorkHelpApi.Models.Output;

public class CommonService : ICommonService
{
	private ICommonRepository _commonRepository;
	private Random _random = new Random();
	
	public CommonService(ICommonRepository commonRepository)
	{
		_commonRepository = commonRepository;
	}

	// Hashing
	public async Task<string> Sha256HashPass(string name, string pass)
	{
		string salt = GenerateSalt(pass.Length);
		await _commonRepository.AddSaltForId(name, salt);
		pass = UniteSaltAndPass(pass, salt);

		using(var sha256 = SHA256.Create())
		{
			var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));
			var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

			return hash;
		}
	}

	private string GenerateSalt(int passLength)
	{
		string strForSalt = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		string result = "";

		int saltLength = _random.Next(8, passLength);
		int index = _random.Next(8, passLength);

		for(int i = 0; i < saltLength; i++)
		{
			result += strForSalt[_random.Next(strForSalt.Length)];
		}

		if(index < 10)
		{
			result += $"0{index}";
		}
		else
		{
			result += $"{index}";
		}

		return result;
	}

	private string UniteSaltAndPass(string pass, string salt)
	{
		int index = Convert.ToInt32($"{salt[salt.Length -2]}{salt[salt.Length -1]}");

		pass = pass.Insert(index, salt);

		return pass;
	}

	// Mappers
	public async Task<WorkOutput> MapWorkEntityToWorkOutput(WorkEntity workEntity)
	{
		return new WorkOutput {Id = workEntity.Id,
								Category = workEntity.Category,
								Name = workEntity.Name,
								Description = workEntity.Description,
								Price = workEntity.Price,
								Link = workEntity.Link};
	}

	public async Task<List<WorkOutput>> MapWorksEntityToWorkOutput(List<WorkEntity> workEntities)
	{
		var localWorkOutputList = new List<WorkOutput>();

		foreach (var workEntity in workEntities)
		{
			localWorkOutputList.Add(
					new WorkOutput { Id = workEntity.Id,
									Category = workEntity.Category, 
									Name = workEntity.Name, 
									Description = workEntity.Description, 
									Price = workEntity.Price, 
									Link = workEntity.Link}
					);
		}

		return localWorkOutputList;
	}

	public async Task<UserOutput> MapUserEntityToUserOutput(UserEntity userEntity)
	{
		return new UserOutput {Id = userEntity.Id,
								Name = userEntity.Name,
								Surname = userEntity.Surname,
								Phone = userEntity.Phone,
								Email = userEntity.Email,
								UserIdTelegram = userEntity.UserIdTelegram,
								NicknameTelegram = userEntity.NicknameTelegram,
								Pass = userEntity.Pass,
								Login = userEntity.Login,
								Role = userEntity.Role,
								Balance = userEntity.Balance,
								Salt = userEntity.Salt,
								RefferUserIdTelegram = userEntity.RefferUserIdTelegram,
								RefferNicknameTelegram = userEntity.RefferNicknameTelegram,
								CompletedTaskCount = userEntity.CompletedTaskCount,
								RefferalsCount = userEntity.RefferalsCount};
	}

	public async Task<List<UserOutput>> MapUserEntitiesToUserOutput(List<UserEntity> userEntities)
	{
		var localUserOutputList = new List<UserOutput>();

		foreach (var userEntity in userEntities)
		{
			localUserOutputList.Add(
					new UserOutput { Id = userEntity.Id,
									Name = userEntity.Name,
									Surname = userEntity.Surname,
									Phone = userEntity.Phone,
									Email = userEntity.Email,
									UserIdTelegram = userEntity.UserIdTelegram,
									NicknameTelegram = userEntity.NicknameTelegram,
									Pass = userEntity.Pass,
									Login = userEntity.Login,
									Role = userEntity.Role,
									Balance = userEntity.Balance,
									Salt = userEntity.Salt,
									RefferUserIdTelegram = userEntity.RefferUserIdTelegram,
									RefferNicknameTelegram = userEntity.RefferNicknameTelegram,
									CompletedTaskCount = userEntity.CompletedTaskCount,
									RefferalsCount = userEntity.RefferalsCount}
					);
		}

		return localUserOutputList;
	}

	public async Task<List<RefferalOutput>> MapRefferalEntitiesToRefferalOutput(List<RefferalEntity> refferalEntities)
	{
		var localRefferalOutputList = new List<RefferalOutput>();

		foreach(var refferalEntity in refferalEntities)
		{
			localRefferalOutputList.Add(
					new RefferalOutput { Id = refferalEntity.Id,
										RefferUserIdTelegram = refferalEntity.RefferUserIdTelegram,
										RefferNicknameTelegram = refferalEntity.RefferNicknameTelegram,
										RefferalUserIdTelegram = refferalEntity.RefferalUserIdTelegram,
										RefferalNicknameTelegram = refferalEntity.RefferalNicknameTelegram,
										Earned = refferalEntity.Earned}
					);
		}

		return localRefferalOutputList;
	}

	public async Task<List<PromocodeOutput>> MapPromocodeEntitiesToPromocodeOutput(List<PromocodeEntity> promocodeEntities)
	{
		var localPromocodOutputList = new List<PromocodeOutput>();

		foreach(var promocodeEntity in promocodeEntities)
		{
			localPromocodOutputList.Add(
					new PromocodeOutput { Id = promocodeEntity.Id,
										Name = promocodeEntity.Name,
										EndDate = promocodeEntity.EndDate,
										Bonus = promocodeEntity.Bonus,
										ActivatedUserId = promocodeEntity.ActivatedUserId}
					);
		}

		return localPromocodOutputList;
	}

	public async Task<PromocodeOutput> MapPromocodeEntityToPromocodeOutput(PromocodeEntity promocodeEntity)
	{
		return new PromocodeOutput { Id = promocodeEntity.Id,
									Name = promocodeEntity.Name,
									EndDate = promocodeEntity.EndDate,
									Bonus = promocodeEntity.Bonus,
									ActivatedUserId = promocodeEntity.ActivatedUserId};
	}
}
