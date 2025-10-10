using Configuration;
using Core.Extensions;
using Core.ServiceModules;
using GameStore.API.Meta.Extensions;
using GameStore.API.Meta.Workers;
using GameStore.Meta.Business.Modules;
using GameStore.Meta.DataAccess;
using GameStore.Meta.SignalR;
using GameStore.Meta.SignalR.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Text;
using GameStore.Meta.Models.Client;
using Microsoft.AspNetCore.Authentication;
using GameStore.API.Meta.Auth.Notification;
using Core.Utilities.ServiceTools;

var builder = WebApplication.CreateBuilder(args);

//await builder.Services.AddRabbitMqAsync();
//builder.Services.AddHostedService<NotificationWorker>();
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

builder.Services.AddAuthentication()
    .AddScheme<NotificationApiKeyAuthenticationOptions, NotificationApiKeyAuthenticationHandler>("NotificationApiKeyScheme", null);

builder.Services.AddScoped<CurrentUser>(i =>
{
    var httpContextAccessor = i.GetService<IHttpContextAccessor>();
    var user = httpContextAccessor?.HttpContext?.User;

    if (user != null && user.Identity.IsAuthenticated)
        return new CurrentUser
        {
            ApiKey = user.Claims.FirstOrDefault(f => f.Type == "ApiKey")?.Value,
            Key = user.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier)?.Value,
            Client = Guid.Parse(user.Claims.FirstOrDefault(f => f.Type == "Client")?.Value),
        };

    return new CurrentUser();
});

builder.Services.AddControllers()
                .AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();
