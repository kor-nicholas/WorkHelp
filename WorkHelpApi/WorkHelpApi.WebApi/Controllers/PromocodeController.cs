using Microsoft.AspNetCore.Mvc;

using WorkHelpApi.Models;
using WorkHelpApi.Models.Output;

using WorkHelpApi.Abstractions.Services;

namespace WorkHelpApi.WebApi.Controllers;

[ApiController]
[Route("promocode")]
public class PromocodeController : ControllerBase
{
	readonly IPromocodeService _promocodeService;

	public PromocodeController(IPromocodeService promocodeService)
	{
		_promocodeService = promocodeService;
	}

	[HttpPut]
	[Route("update/{name}/{userId}")]
	public async Task<ActionResult<BoolModelWithErrorMessage>> UpdatePromocode([FromRoute] string name, string userId)
	{
		var result = await _promocodeService.UpdatePromocode(name, userId);

		if (result.IsError)
		{
			return BadRequest(result);
		}
		else
		{
			return Ok(result);
		}

	}

	[HttpPost]
	[Route("add/{name}/{endDate}/{bonus}")]
	public async Task<ActionResult<BoolModelWithErrorMessage>> AddPromocode([FromRoute]string name, DateTime endDate, int bonus)
	{
		// !!! FORMAT DATE : YYYY-MM-DD !!!
		var result = await _promocodeService.AddPromocode(name, endDate, bonus);

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
	public async Task<ActionResult<BoolModelWithErrorMessage>> DeletePromocode([FromRoute]string name)
	{
		var result = await _promocodeService.DeletePromocode(name);

		if (result.IsError)
		{
			return BadRequest(result);
		}
		else
		{
			return Ok(result);
		}
	}

	[HttpGet]
	[Route("promocodes")]
	public async Task<ActionResult<List<PromocodeOutput>>> GetPromocodes()
	{
		try
		{
			var result = await _promocodeService.GetPromocodes();

			return Ok(result);
		}
		catch(Exception e)
		{
			return BadRequest(e.Message);
		}
	}

	[HttpGet]
	[Route("get/{name}")]
	public async Task<ActionResult<PromocodeOutput>> GetPromocodeForName([FromRoute] string name)
	{
		try
		{
			var result = await _promocodeService.GetPromocodeForName(name);

			return Ok(result);
		}
		catch(Exception e)
		{
			return BadRequest(e.Message);
		}
	}
}




