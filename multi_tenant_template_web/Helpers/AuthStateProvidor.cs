using CMS.Web.Services;
using Common.Api.Responses;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CMS.Web.Helpers
{
    public class AuthStateProvidor : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationState _anonymous;
        public AuthStateProvidor(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var access = await _localStorage.GetItem<string>("accessToken");
            var refresh = await _localStorage.GetItem<string>("refreshToken");
            if (string.IsNullOrWhiteSpace(access))
                return _anonymous;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ExtensionMethods.ParseClaimsFromJwt(access), "jwt")));
        }

        public void NotifyUserAuthentication(string email)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "apiauth"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
