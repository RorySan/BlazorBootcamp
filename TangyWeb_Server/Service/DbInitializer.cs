using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tangy_Common;
using Tangy_DataAccess.Data;
using TangyWeb_Server.Service.IService;

namespace TangyWeb_Server.Service;

public class DbInitializer : IDbInitializer
{
	// in a real application we would create a repository for users
	// this is an example on how to use ApplicationDbContext directly
	private readonly UserManager<IdentityUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly ApplicationDbContext _db;
	public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
	{
		_userManager = userManager;
		_roleManager = roleManager;
		_db = db;
	}
	
	public void Initialize()
	{
		try
		{
			if (_db.Database.GetPendingMigrations().Count() > 0)
			{
				_db.Database.Migrate();
			}
			// GetAwaiter().GetResult() allows is similar to awaiting but it does not need to be inside an async method
			// It has implications, so you cannot just change one for the other. More in the docs.
			if (!_roleManager.RoleExistsAsync(StaticDetails.RoleAdmin).GetAwaiter().GetResult())
			{
				_roleManager.CreateAsync(new IdentityRole(StaticDetails.RoleAdmin)).GetAwaiter().GetResult();
				_roleManager.CreateAsync(new IdentityRole(StaticDetails.RoleCustomer)).GetAwaiter().GetResult();
			}
			else
			{
				return;
			}

			var user = new IdentityUser
			{
				UserName = "admin@rory.es",
				Email = "admin@rory.es",
				EmailConfirmed = true
			};

			_userManager.CreateAsync(user, "Admin@123").GetAwaiter().GetResult();

			_userManager.AddToRoleAsync(user, StaticDetails.RoleAdmin).GetAwaiter().GetResult();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}
}