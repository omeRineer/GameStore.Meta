using GameStore.Meta.Business.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace GameStore.API.Meta.Auth
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        const string ApiKeyHeaderName = "Hub-Api-Key";
        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var headers))
            //    return AuthenticateResult.Fail("API Key not provided.");


            //var providedApiKey = headers.FirstOrDefault();

            //if (client != null)
            //{
            //    var claims = new[] { new Claim(ClaimTypes.Name, client.Key) };
            //    var identity = new ClaimsIdentity(claims, Scheme.Name);
            //    var principal = new ClaimsPrincipal(identity);
            //    var ticket = new AuthenticationTicket(principal, Scheme.Name);
            //    return AuthenticateResult.Success(ticket);
            //}

            return AuthenticateResult.Fail("Invalid API Key.");
        }
    }
}
