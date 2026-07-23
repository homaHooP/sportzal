using FluentValidation;
using GymAppApi.Application;
using GymAppApi.Application.Common.Behaviors;
using GymAppApi.Data;
using GymAppApi.Domain.Constants;
using GymAppApi.Domain.Models;
using GymAppApi.Middleware;
using GymAppApi.Services;
using GymAppApi.Services.Token;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Validation
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CurrentUserService>();

builder.Services.AddScoped(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
#endregion

builder.Services.AddControllers();

#region DB
var connectionString = builder.Configuration.GetConnectionString("localMSSQL");
builder.Services.AddDbContext<GymAppDbContext>(options =>
    options.UseSqlServer(connectionString));
#endregion

#region Identity and JWT
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
})
.AddEntityFrameworkStores<GymAppDbContext>()
.AddDefaultTokenProviders();

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
    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = context.AuthenticateFailure is SecurityTokenExpiredException
                    ? "Token has expired"
                    : "Authentication token is missing or invalid",
                Instance = context.Request.Path
            };

            await context.Response.WriteAsJsonAsync(problem);
        },
        OnForbidden = async context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Forbidden",
                Detail = "You do not have permission to perform this action",
                Instance = context.Request.Path
            };

            await context.Response.WriteAsJsonAsync(problem);
        }
    };
});

builder.Services.AddScoped<ITokenService, TokenService>();

#endregion

#region Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireManager", policy =>
        policy.RequireRole(RoleNames.Manager, RoleNames.HeadManager));

    options.AddPolicy("RequireHeadManager", policy =>
        policy.RequireRole(RoleNames.HeadManager));

    options.AddPolicy("RequireTrainer", policy =>
        policy.RequireRole(RoleNames.Trainer, RoleNames.Manager, RoleNames.HeadManager));
});
#endregion

#region MediatR 
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
#endregion


builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
              .AllowCredentials()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddScoped<GymManagerDemotionService, GymManagerDemotionService>();
var app = builder.Build();

app.UseCors("Frontend");

#region Roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    string[] roles = { RoleNames.Client, RoleNames.Trainer, RoleNames.Manager, RoleNames.HeadManager };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole<Guid>(role));
    }
}
#endregion

if (app.Environment.IsDevelopment())
{

}

app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();