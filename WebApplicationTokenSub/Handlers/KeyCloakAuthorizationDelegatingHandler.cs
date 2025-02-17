using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using WebApplicationTokenSub.Entities;
using WebApplicationTokenSub.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;


public class AuthToken
{
    public string AccessToken { get; set; }
}


namespace WebApplicationTokenSub.Handlers
{
    //public class KeyCloakAuthorizationDelegatingHandler : DelegatingHandler
    //{

    //}

    public class KeyCloakAuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly KeycloakOptions _keycloakOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public KeyCloakAuthorizationDelegatingHandler(
            IOptions<KeycloakOptions> keycloakOptions, IHttpContextAccessor httpContextAccessor)
        {
            _keycloakOptions = keycloakOptions.Value;
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            //var authToken = await GetAccessTokenAsync(cancellationToken);
            var tokenValue = await GetTokenString();

            //request.Headers.Authorization = new AuthenticationHeaderValue(
            //    JwtBearerDefaults.AuthenticationScheme,
            //    authToken.AccessToken);

            request.Headers.Authorization = new AuthenticationHeaderValue(
               JwtBearerDefaults.AuthenticationScheme,
               tokenValue);

            var httpResponseMessage = await base.SendAsync(
                request,
                cancellationToken);

            httpResponseMessage.EnsureSuccessStatusCode();

           
            return httpResponseMessage;

            
        }

        private async Task<string?> GetTokenString()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated)
            {
               
                var token = await httpContext!.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token");
                return token;
            }
            

            return null;
        }

        private async Task<AuthToken> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            //var params = new KeyValuePair<string, string>[]
            //{
            //new("client_id", _keycloakOptions.AdminClientId),
            //new("client_secret", _keycloakOptions.AdminClientSecret),
            //new("scope", "openid email"),
            //new("grant_type", "client_credentials")
            //};

            var data = new Dictionary<string, string>();
            data.Add("client_id", _keycloakOptions.AdminClientId);
            data.Add("client_secret", _keycloakOptions.AdminClientSecret);
            data.Add("scope", "openid email");
            data.Add("grant_type", "client_credentials");

            var content = new FormUrlEncodedContent(data);

            var authRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_keycloakOptions.TokenUrl))
            {
                Content = content
            };

            var response = await base.SendAsync(authRequest, cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AuthToken>() ?? throw new ApplicationException();






        }


    }



}
