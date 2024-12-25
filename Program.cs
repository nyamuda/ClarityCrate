using Clarity_Crate.Components;
using Clarity_Crate.Components.Account;
using Clarity_Crate.Data;
using Clarity_Crate.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Syncfusion.Blazor;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();






builder.Services.AddAuthentication(options =>
	{
		options.DefaultScheme = IdentityConstants.ApplicationScheme;
		options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
	})
	//Add Google Login
	.AddGoogle(options =>
	{
		options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
		options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

	})
	.AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddRoles<IdentityRole>()//add this to manage user roles
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddSignInManager()
	.AddDefaultTokenProviders();

//builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

//email
builder.Services.AddScoped<EmailSender>();
builder.Services.AddMudServices();
builder.Services.AddScoped<SubjectService>();
builder.Services.AddScoped<CurriculumService>();
builder.Services.AddScoped<TopicService>();
builder.Services.AddScoped<TermService>();
builder.Services.AddScoped<LevelService>();
builder.Services.AddScoped<TemplateService>();
builder.Services.AddScoped<AppService>();
builder.Services.AddScoped<DefinitionService>();
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<SummarizationService>();

// Register OpenAIService
builder.Services.AddHttpClient<OpenAIService>();

//Register Syncfusion
builder.Services.AddSyncfusionBlazor();
var syncfusionLicenseKey = builder.Configuration["Authentication:Syncfusion:LicenseKey"];
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicenseKey);
//added for the syncfusion pdf viewer server
builder.Services.AddMemoryCache(); // Required for Syncfusion PDF Viewer Server
builder.Services.AddServerSideBlazor().AddHubOptions(options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024 * 100; // 100 MB
    options.ClientTimeoutInterval = TimeSpan.FromMinutes(5); // Adjust as needed
    options.KeepAliveInterval = TimeSpan.FromMinutes(2); // Adjust to prevent timeouts
});





builder.WebHost.UseStaticWebAssets();



var app = builder.Build();




// Seed roles and assign the "Admin" role to a specific user
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	try
	{
		await SeedRoles.CreateRoles(services);
	}
	catch (Exception ex)
	{
		// Log the error or handle it as needed
		Console.WriteLine($"Error seeding roles: {ex.Message}");
	}
}





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
