

using Clarity_Crate.Data;
using Microsoft.AspNetCore.Identity;

public static class SeedRoles
{
	public static async Task CreateRoles(IServiceProvider serviceProvider)
	{
		var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
		var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

		// Check if the Admin role exists, and create it if it doesn't
		if (!await roleManager.RoleExistsAsync("Admin"))
		{
			await roleManager.CreateAsync(new IdentityRole("Admin"));
		}

		// Check if the User role exists, and create it if it doesn't
		if (!await roleManager.RoleExistsAsync("User"))
		{
			await roleManager.CreateAsync(new IdentityRole("User"));
		}


		//the email or username of the user you want to assign the Admin role to
		string adminEmail = "ptn1234@gmail.com";
		var user = await userManager.FindByEmailAsync(adminEmail);

		if (user != null)
		{
			// Assign the Admin role to the user if they are not already in it
			if (!await userManager.IsInRoleAsync(user, "Admin"))
			{
				await userManager.AddToRoleAsync(user, "Admin");

				Console.WriteLine("Role added to ptn1234");
			}
		}
	}
}