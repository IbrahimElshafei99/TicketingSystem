using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Ticketing.Business.Interfaces;
using Ticketing.Business.Services;
using Ticketing.Core.Context;
using Ticketing.Core.Interfaces;
using Ticketing.Core.Repos;
using TicketingSystem;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager config = builder.Configuration;
builder.Services.Configure<JwtOptions>(config.GetSection("JWT"));


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt=> opt.UseSqlServer(config.GetConnectionString("ConnStr")));
builder.Services.AddSingleton<IConfiguration, ConfigurationManager>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITicketRepo, TicketRepo>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped(typeof(IGenericsRepo<>), typeof(GenericsRepo<>));
builder.Services.AddScoped<HttpClient>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
                .AddJwtBearer(option =>
                {
                    option.SaveToken = true;
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = config["JWT:Issure"],
                        ValidateAudience = true,
                        ValidAudience = config["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]))
                    };
                });

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("AdminOnly", opt => opt.RequireRole("Manager", "Support Team"));
});

builder.Services.AddCors();/* (options =>
{
    options.AddPolicy("myPolicy", opt =>
    {
        opt.WithOrigins("https://localhost:7152/DynamicTickets").AllowAnyMethod().AllowAnyHeader();
    });
});*/

var logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();
//builder.Logging.AddSerilog(logger);
builder.Host.UseSerilog(logger);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();

app.UseCors(opt=>opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
