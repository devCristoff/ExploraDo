using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Project2030.API.Extensions;
using Project2030.API.Middleware;
using Project2030.Domain.Entities;
using Project2030.Infrastructure.Data;
using Project2030.Infrastructure.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// ─── Servicios 
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddIdentityConfiguration();
builder.Services.AddControllers();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddInfrastructureServices();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// ─── Pipeline 
WebApplication app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseSwaggerConfiguration();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ─── Seed de datos iniciales 
using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;

    AppDbContext dbContext = services.GetRequiredService<AppDbContext>();
    RoleManager<ApplicationRole> roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
    UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    await DbSeeder.SeedAsync(roleManager, userManager, dbContext);
}

app.Run();
