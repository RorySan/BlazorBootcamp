using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tangy_Common;
using Tangy_DataAccess;
using Tangy_Models;

namespace TangyWeb_API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController : Controller
{
	private readonly SignInManager<IdentityUser> _signInManager;
	private readonly UserManager<IdentityUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	public AccountController(
		UserManager<IdentityUser> userManager,
		SignInManager<IdentityUser> signInManager,
		RoleManager<IdentityRole> roleManager)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_roleManager = roleManager;
	}

	public async Task<IActionResult> SignUp([FromBody] SignUpRequestDTO signUpRequestDTO)
	{
		if (signUpRequestDTO == null || !ModelState.IsValid)
		{
			return BadRequest();
		}
		
		var user = new ApplicationUser
		{
			UserName = signUpRequestDTO.Email,
			Email = signUpRequestDTO.Email,
			Name = signUpRequestDTO.Name,
			PhoneNumber = signUpRequestDTO.PhoneNumber,
			EmailConfirmed = true
		};

		var result = await _userManager.CreateAsync(user, signUpRequestDTO.Password);

		if (!result.Succeeded)
		{
			return BadRequest(new SignUpResponseDTO()
			{
				IsRegistrationSuccessful = false,
				Errors = result.Errors.Select(error=> error.Description)
			});
		}

		var roleResult = await _userManager.AddToRoleAsync(user, StaticDetails.RoleCustomer);
		if (!roleResult.Succeeded)
		{
			return BadRequest(new SignUpResponseDTO()
			{
				IsRegistrationSuccessful = false,
				Errors = result.Errors.Select(error=> error.Description)
			});
		}

		return StatusCode(201);
	}
}