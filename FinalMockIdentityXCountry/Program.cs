using FinalMockIdentityXCountry.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;
using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Classes;
using Microsoft.AspNetCore.Identity.UI.Services;
using FinalMockIdentityXCountry.Models.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 30)); //current version of mysql - 8.0.30

builder.Services.AddDbContext<XCountryDbContext>(options => options.UseMySql(connectionString, serverVersion));

//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<XCountryDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IEmailSender, FakeEmailSender>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login"; 
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

var app = builder.Build();
Configure(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Welcome}/{controller=Home}/{action=Index}/{id?}");

app.Run();


void Configure(WebApplication host)
{
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        var dbContext = services.GetRequiredService<XCountryDbContext>();

        if (dbContext.Database.IsMySql())
        {
            dbContext.Database.Migrate();
        }

        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        XCountryDbContext.SeedData(userManager, roleManager);
    }
    catch (Exception ex)
    {
        //Log some error
        throw;
    }
}