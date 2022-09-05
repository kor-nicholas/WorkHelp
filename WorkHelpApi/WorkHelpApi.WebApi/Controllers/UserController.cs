using Microsoft.AspNetCore.Mvc;

using WorkHelpApi.Models;
using WorkHelpApi.Models.Output;
using WorkHelpApi.Models.Input;
using WorkHelpApi.Abstractions.Services;

namespace WorkHelpApi.WebApi.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
	private IUserService _userService;

	public UserController(IUserService userService)
	{
		_userService = userService;
	}

	[HttpGet]
	[Route("users")]
	public async Task<ActionResult<IEnumerable<UserOutput>>> GetAllUsers()
	{
		try
		{
			var result = await _userService.GetAllUsers();

			return Ok(result);
		}
		catch(Exception e)
		{
			return BadRequest(e.Message);
		}
	}

	[HttpGet]
	[Route("get/{userId}")]
	public async Task<ActionResult<UserOutput>> GetUserByUserId([FromRoute]string userId)
	{
		try
		{
			var result = await _userService.GetUserByUserId(userId);

			return Ok(result);
		}
		catch(Exception e)
		{
			return BadRequest(e.Message);
		}
	}

	[HttpPost]
	[Route("add")]
	public async Task<ActionResult<BoolModelWithErrorMessage>> AddUser([FromBody] UserInput userInput)
	{
		var result = await _userService.AddUser(userInput);

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
	[Route("add-reffer-data")]
	public async Task<ActionResult<BoolModelWithErrorMessage>> AddRefferData([FromBody] RefferalInput refferalInput)
	{
		var result = await _userService.AddRefferData(refferalInput);

		if(result.IsError)
		{
			return BadRequest(result);
		}
		else
		{
			return Ok(result);
		}
	}

	[HttpDelete]
	[Route("delete/{id}")]
	public async Task<ActionResult<BoolModelWithErrorMessage>> DeleteUserById([FromRoute] int id)
	{
		var result = await _userService.DeleteUserById(id);

		if(result.IsError)
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
	public async Task<ActionResult<BoolModelWithErrorMessage>> ClearUsers()
	{
		var result = await _userService.ClearUsers();

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
	public async Task<ActionResult<BoolModelWithErrorMessage>> UpdateUser([FromBody] UserInput userInput)
	{
		var result = await _userService.UpdateUser(userInput);

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
	[Route("update-balance/{userId}/{balance}")]
	public async Task<ActionResult<BoolModelWithErrorMessage>> UpdateBalance([FromRoute] string userId, int balance)
	{
		var result = await _userService.UpdateBalance(userId, balance);

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



