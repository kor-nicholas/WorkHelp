using WorkHelpApi.Models.Entities;
using WorkHelpApi.Models.Input;
using WorkHelpApi.Models.Output;

namespace WorkHelpApi.Abstractions.Services;

public interface ICommonService
{
	// Hashing
	Task<string> Sha256HashPass(string name, string pass);

	// Mappers
	Task<List<WorkOutput>> MapWorksEntityToWorkOutput(List<WorkEntity> workEntities);
	Task<WorkOutput> MapWorkEntityToWorkOutput(WorkEntity workEntity);
	Task<List<UserOutput>> MapUserEntitiesToUserOutput(List<UserEntity> userEntities);
	Task<UserOutput> MapUserEntityToUserOutput(UserEntity userEntity);
	Task<List<RefferalOutput>> MapRefferalEntitiesToRefferalOutput(List<RefferalEntity> refferalEntities);
	Task<List<PromocodeOutput>> MapPromocodeEntitiesToPromocodeOutput(List<PromocodeEntity> promocodeEntities);
	Task<PromocodeOutput> MapPromocodeEntityToPromocodeOutput(PromocodeEntity promocodeEntity);

	// 
}


