using WorkHelpApi.Models.Entities;
using WorkHelpApi.Models;
using WorkHelpApi.Models.Input;

using WorkHelpApi.Abstractions.Repositories;

using WorkHelpApi.Core;

using System.Collections.Generic;

namespace WorkHelpApi.Services.Repositories;

public class WorkRepository : IWorkRepository
{
	private MsSqlDbContext _dbContext;

	public WorkRepository(MsSqlDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<List<WorkEntity>> GetAllWorks()
	{
		return _dbContext.Work.ToList();
	}

	public async Task<WorkEntity> GetWorkById(int id)
	{
		return _dbContext.Work.FirstOrDefault(work => work.Id == id);
	}

	public async Task<BoolModelWithErrorMessage> AddWork(WorkInput workInput)
	{
		try
		{
			_dbContext.Work.Add(new WorkEntity {
					Name = workInput.Name,
					Category = workInput.Category,
					Description = workInput.Description,
					Price = workInput.Price,
					Link = workInput.Link});

			_dbContext.SaveChanges();

			return new BoolModelWithErrorMessage { IsError = false };
		}
		catch (Exception e)
		{
			return new BoolModelWithErrorMessage { IsError = true, ErrorMessage = e.Message };
		}
	}

	public async Task<BoolModelWithErrorMessage> DeleteWorkByName(string name)
	{
		try
		{
			var work = _dbContext.Work.Single(work => work.Name == name);
			_dbContext.Remove(work);

			_dbContext.SaveChanges();

			return new BoolModelWithErrorMessage { IsError = false };
		}
		catch (Exception e)
		{
			return new BoolModelWithErrorMessage { IsError = true, ErrorMessage = e.Message };
		}
	}

	public async Task<BoolModelWithErrorMessage> ClearWorks()
	{
		try
		{
			foreach (var item in _dbContext.Work)
			{
				_dbContext.Work.Remove(item);
			}

			_dbContext.SaveChanges();

			return new BoolModelWithErrorMessage { IsError = false };
		}
		catch (Exception e)
		{
			return new BoolModelWithErrorMessage { IsError = true, ErrorMessage = e.Message };
		}
	}

	public async Task<BoolModelWithErrorMessage> UpdateWork(WorkInput workInput)
	{
		try
		{
			var work = _dbContext.Work.Single(work => work.Name == workInput.Name);

			work.Category = workInput.Category;
			work.Name = workInput.Name;
			work.Description = workInput.Description;
			work.Price = workInput.Price;
			work.Link = workInput.Link;

			_dbContext.SaveChanges();

			return new BoolModelWithErrorMessage { IsError = false };
		}
		catch (Exception e)
		{
			return new BoolModelWithErrorMessage { IsError = true, ErrorMessage = e.Message };
		}
	}
}
