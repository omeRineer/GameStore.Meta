using Microsoft.AspNetCore.Authentication;

namespace GameStore.API.Meta.Auth
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string ApiKeyHeaderName = "Hub-Api-Key";
    }
}
