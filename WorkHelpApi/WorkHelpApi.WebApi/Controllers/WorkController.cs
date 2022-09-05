using Microsoft.AspNetCore.Mvc;

using WorkHelpApi.Models;
using WorkHelpApi.Models.Output;
using WorkHelpApi.Models.Input;

using WorkHelpApi.Abstractions.Services;

namespace WorkHelpApi.WebApi.Controllers;

[ApiController]
[Route("work")]
public class WorkController : ControllerBase
{
	private IWorkService _workService;

	public WorkController(IWorkService workService)
	{
		_workService = workService;
	}

	[HttpGet]
	[Route("works")]
	public async Task<ActionResult<List<WorkOutput>>> GetAllWorks()
	{
		var result = await _workService.GetAllWorks();

		return Ok(result);
	}

	[HttpGet]
	[Route("get/{id}")]
	public async Task<ActionResult<WorkOutput>> GetWorkById([FromRoute]int id)
	{
		var result = await _workService.GetWorkById(id);

		return Ok(result);
	}

	[HttpPost]
	[Route("add")]
	public async Task<ActionResult<BoolModelWithErrorMessage>> AddWork([FromBody] WorkInput workInput)
	{
		var result = await _workService.AddWork(workInput);

		if (result.IsError)
		{
			return BadRequest(result);
		}
		else
		{
			return Ok(result);
		}

	}

	[HttpDelete]
	[Route("delete/{name}")]
	public async Task<ActionResult<BoolModelWithErrorMessage>> DeleteWorkByName([FromRoute] string name)
	{
		var result = await _workService.DeleteWorkByName(name);

		if (result.IsError)
		{
			return BadRequest(result);
		}
		else
		{
			return Ok(result);
		}
	}

	[HttpDelete]
	[Route("clear")]
	public async Task<ActionResult<BoolModelWithErrorMessage>> ClearWorks()
	{
		var result = await _workService.ClearWorks();

		if (result.IsError)
		{
			return BadRequest(result);
		}
		else
		{
			return Ok(result);
		}
	}

	[HttpPut]
	[Route("update")]
	public async Task<ActionResult<BoolModelWithErrorMessage>> UpdateWork([FromBody] WorkInput workInput)
	{
		var result = await _workService.UpdateWork(workInput);

		if (result.IsError)
		{
			return BadRequest(result);
		}
		else
		{
			return Ok(result);
		}
	}
}
