using GameStore.Meta.Business.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace GameStore.API.Meta.Auth.Notification
{
    public class NotificationApiKeyAuthenticationHandler : AuthenticationHandler<NotificationApiKeyAuthenticationOptions>
    {
        const string ApiKeyHeaderName = "Notification-Api-Key";
        readonly NotificationSubscriberService NotificationSubscriberService;
        public NotificationApiKeyAuthenticationHandler(IOptionsMonitor<NotificationApiKeyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, NotificationSubscriberService notificationSubscriberService) : base(options, logger, encoder, clock)
        {
            NotificationSubscriberService = notificationSubscriberService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var headers) &
                !Request.Query.TryGetValue(ApiKeyHeaderName, out var queries))
                return AuthenticateResult.Fail("API Key not provided.");

            var providedApiKey = headers.FirstOrDefault() ?? queries.FirstOrDefault();
            var subscriber = (await NotificationSubscriberService.GetSubscriberAsync(providedApiKey)).Data;

            if (subscriber == null)
                return AuthenticateResult.Fail("Invalid API Key.");

            if (subscriber.ExpireDate != null && subscriber.ExpireDate < DateTime.Now)
                return AuthenticateResult.Fail("API Key is expired.");

            var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, subscriber.Key),
                    new Claim("ApiKey", subscriber.ApiKey),
                    new Claim("Client", subscriber.Client.ToString())
                };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
