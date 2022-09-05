using Microsoft.AspNetCore.Mvc;

using WorkHelpApi.Models;
using WorkHelpApi.Models.Output;
using WorkHelpApi.Models.Input;
using WorkHelpApi.Abstractions.Services;

namespace WorkHelpApi.WebApi.Controllers;

[ApiController]
[Route("refferal")]
public class RefferalController : ControllerBase
{
	private IRefferalService _refferalService;

	public RefferalController(IRefferalService refferalService)
	{
		_refferalService = refferalService;
	}

	[HttpGet]
	[Route("get/{userId}")]
	public async Task<ActionResult<IEnumerable<RefferalOutput>>> GetRefferalsByUserId([FromRoute]string userId)
	{
		try
		{
			var result = await _refferalService.GetRefferalsByUserId(userId);

			return Ok(result);
		}
		catch(Exception e)
		{
			return BadRequest(e.Message);
		}
	}

	[HttpPost]
	[Route("add")]
	public async Task<ActionResult<BoolModelWithErrorMessage>> AddRefferal([FromBody] RefferalInput refferalInput)
	{
		var result = await _refferalService.AddRefferal(refferalInput);

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
	public async Task<ActionResult<BoolModelWithErrorMessage>> UpdateRefferal([FromBody] RefferalInput refferalInput)
	{
		var result = await _refferalService.UpdateRefferal(refferalInput);

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



