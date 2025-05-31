using Configuration;
using Core.Extensions;
using Core.ServiceModules;
using GameStore.API.Meta.Extensions;
using GameStore.API.Meta.Workers;
using GameStore.Meta.Business.Modules;
using GameStore.Meta.DataAccess;
using GameStore.Meta.SignalR;
using GameStore.Meta.SignalR.Hubs;
using MeArch.Module.Security.Model.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

await builder.Services.AddRabbitMqAsync();
builder.Services.AddHostedService<NotificationWorker>();
builder.Services.AddServiceModules(new IServiceModule[]
{
    new RepositoryServiceModule(),
    new BusinessServiceModule(),
    new SignalRModule()
});


builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
                            options.AddDefaultPolicy(builder =>
                            builder.AllowAnyHeader()
                                   .AllowAnyMethod()
                                   .AllowAnyOrigin()));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(opt =>
                            {
                                opt.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,

                                    ValidIssuer = MetaConfiguration.TokenOptions.Issuer,
                                    ValidAudience = MetaConfiguration.TokenOptions.Audience,
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(MetaConfiguration.TokenOptions.SecurityKey))

                                };

                                opt.Events = new JwtBearerEvents
                                {
                                    OnMessageReceived = (context) =>
                                    {
                                        var accessToken = context.Request.Query["access_token"];
                                        var path = context.Request.Path;

                                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                                            context.Token = accessToken;

                                        return Task.CompletedTask;
                                    }
                                };
                            });
builder.Services.AddScoped<CurrentUser>(i =>
{
    var httpContextAccessor = i.GetService<IHttpContextAccessor>();
    var user = httpContextAccessor.HttpContext?.User;

    if (user != null && user.Identity.IsAuthenticated)
        return new CurrentUser
        {
            Id = Guid.Parse(user.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier)?.Value),
            Name = user.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Name)?.Value,
            Phone = user.Claims.FirstOrDefault(f => f.Type == ClaimTypes.MobilePhone)?.Value,
            Email = user.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Email)?.Value,
            Roles = user.Claims.Where(f => f.Type == ClaimTypes.Role)?.Select(s => s.Value).ToArray(),
            Permissions = user.Claims.Where(f => f.Type == "Permission")?.Select(s => s.Value).ToArray(),
            IsAuthenticated = user.Identity.IsAuthenticated
        };

    return new CurrentUser();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();
