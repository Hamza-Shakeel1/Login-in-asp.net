using Logins.Abstraction;
using Microsoft.AspNetCore.Authentication.Cookies;
using Logins.Service;
using Microsoft.EntityFrameworkCore;
using Logins.Models;

internal class Program
{
    private static string AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Register the HmsContext with the connection string from appsettings.json
        builder.Services.AddDbContext<HmsContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Register application services
        builder.Services.AddScoped<IContextServices, ContextServices>();
        builder.Services.AddScoped<IConfigurationServices, ConfigurationService>();
        builder.Services.AddScoped<IUserServices, UserServices>();

        // Configure cookie-based authentication with expiry time from appsettings.json
        int authExpiryTime = builder.Configuration.GetValue<int>("Authentication:ExpiryTime");
        builder.Services.AddAuthentication(AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Authentication/Login";  // Redirect to login page on authentication failure
                options.ExpireTimeSpan = TimeSpan.FromMinutes(authExpiryTime);  // Set cookie expiration time based on config
            });

        // Add HTTP context accessor for user context
        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();  // Use HSTS for production environments
        }

        app.UseHttpsRedirection();  // Force HTTPS

        app.UseStaticFiles();  // Serve static files

        app.UseRouting();  // Enable routing

        app.UseAuthentication();  // Enable authentication middleware
        app.UseAuthorization();  // Enable authorization middleware

        // Configure default route for the application
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Authentication}/{action=Login}/{id?}");

        app.Run();
    }
}


//Scaffold-DbContext "Server=.;Database=Login;Trusted_Connection=True;Integrated Security=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context HmsContext -Force