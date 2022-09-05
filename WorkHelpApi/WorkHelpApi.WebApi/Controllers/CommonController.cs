using Microsoft.AspNetCore.Mvc;

namespace WorkHelpApi.WebApi.Controllers;

[ApiController]
[Route("common")]
public class CommonController : ControllerBase
{
	[Route("test")]
	public async Task<string> Test()
	{
		using(var client = new HttpClient())
		{
			var endPoint = new Uri("https://translate.yandex.com/developers/keys");
			var result = client.GetAsync(endPoint).Result.Content.ReadAsStringAsync().Result;
			return result;
		}
	}

	/* [Http] */
	/* [Route("")] */
	/* public async Task<> Registration() */
	/* { */
	/* 	var result = await _userService.UpdateUser(userInput); */

	/* 	if (result.IsError) */
	/* 	{ */
	/* 		return BadRequest(result); */
	/* 	} */
	/* 	else */
	/* 	{ */
	/* 		return Ok(result); */
	/* 	} */
	/* } */

	/* [Http] */
	/* [Route("")] */
	/* public async Task<> Login() */
	/* { */
	/* 	var result = await _userService.UpdateUser(userInput); */

	/* 	if (result.IsError) */
	/* 	{ */
	/* 		return BadRequest(result); */
	/* 	} */
	/* 	else */
	/* 	{ */
	/* 		return Ok(result); */
	/* 	} */
	/* } */

	/* [Http] */
	/* [Route("")] */
	/* public async Task<> Logout() */
	/* { */
	/* 	var result = await _userService.UpdateUser(userInput); */

	/* 	if (result.IsError) */
	/* 	{ */
	/* 		return BadRequest(result); */
	/* 	} */
	/* 	else */
	/* 	{ */
	/* 		return Ok(result); */
	/* 	} */
	/* } */
}



