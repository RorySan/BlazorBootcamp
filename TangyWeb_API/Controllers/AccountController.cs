﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Tangy_Common;
using Tangy_DataAccess;
using Tangy_Models;
using TangyWeb_API.Helper;

namespace TangyWeb_API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController : Controller
{
	private readonly SignInManager<ApplicationUser> _signInManager;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly APISettings _apiSettings;

	public AccountController(
		UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager,
		RoleManager<IdentityRole> roleManager,
		IOptions<APISettings> options)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_roleManager = roleManager;
		_apiSettings = options.Value;
	}
	[HttpPost]
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
	
	[HttpPost]
	public async Task<IActionResult> SignIn([FromBody] SignInRequestDTO signInRequestDTO)
	{
		if (signInRequestDTO == null || !ModelState.IsValid)
		{
			return BadRequest();
		}

		var result = await _signInManager.PasswordSignInAsync(signInRequestDTO.UserName, 
			signInRequestDTO.Password, false,false);
		if (result.Succeeded)
		{
			var user = await _userManager.FindByNameAsync(signInRequestDTO.UserName);
			if (user == null)
			{
				return Unauthorized(new SignInResponseDTO
				{
					IsAuthSuccessful = false,
					ErrorMessage = "Invalid Authentication"
				});
			}
			
			// everything is valid and we need to login
			var signinCredentials = GetSigningCredentials();
			var claims = await GetClaims(user);

			// fill token options with everything we gathered
			var tokenOptions = new JwtSecurityToken(
				issuer: _apiSettings.ValidIssuer,
				audience: _apiSettings.ValidAudience,
				claims: claims,
				expires: DateTime.Now.AddDays(30),
				signingCredentials: signinCredentials);

			//create security token
			var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions); // this will go to the SignInResponseDTO

			return Ok(new SignInResponseDTO
			{
				IsAuthSuccessful = true,
				Token = token,
				UserDTO = new UserDTO
				{
					Name = user.Name,
					Id = user.Id,
					Email = user.Email,
					PhoneNumber = user.PhoneNumber
				}
			});
		}
		else
		{
			return Unauthorized(new SignInResponseDTO
			{
				IsAuthSuccessful = false,
				ErrorMessage = "Invalid Authentication"
			});
		}
		return StatusCode(201);
	}

	private SigningCredentials GetSigningCredentials()
	{
		var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiSettings.SecretKey));
		return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
	}

	private async Task<List<Claim>> GetClaims(ApplicationUser user)
	{
		var claims = new List<Claim>
		{
			new(ClaimTypes.Name, user.Email),
			new(ClaimTypes.Email, user.Email),
			new("Id",
				user.Id) // apart from built in, default claims (name, email), we can add our own custom claims
		};
		var roles = await _userManager.GetRolesAsync(await _userManager.FindByEmailAsync(user.Email));
		
		foreach (var role in roles)
		{
			claims.Add(new Claim(ClaimTypes.Role, role));
		}

		return claims;
	}
}