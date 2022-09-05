using WorkHelpApi.Models;

namespace WorkHelpApi.Abstractions.Repositories;

public interface ICommonRepository
{
	Task<BoolModelWithErrorMessage> AddSaltForId(string name, string salt);
}



