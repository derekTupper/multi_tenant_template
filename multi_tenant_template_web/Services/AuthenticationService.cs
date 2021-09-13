using Common.Api.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;
using CMS.Web.Helpers;
using System.Text.Json;
using System.Text;
using System.Net.Http;

namespace CMS.Web.Services
{
    public interface IAuthenticationService
    {
        Task Login(string username, string password);
        Task Logout();
        //Task<string> RefreshToken();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private IHttpService _httpService;
        private NavigationManager _navigationManager;
        private ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authState;

        public LoginResult LoginResult { get; private set; }

        public AuthenticationService(
            IHttpService httpService,
            NavigationManager navigationManager,
            ILocalStorageService localStorageService,
            AuthenticationStateProvider authState
        )
        {
            _httpService = httpService;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
            _authState = authState;
        }

        public async Task Login(string username, string password)
        {
            LoginResult = await _httpService.Post<LoginResult>("security/login", new { username, password });
            
            await _localStorageService.SetItem("accessToken", LoginResult.AccessToken);
            await _localStorageService.SetItem("refreshToken", LoginResult.RefreshToken);

            ((AuthStateProvidor)_authState).NotifyUserAuthentication(username);

            _navigationManager.NavigateTo("/");
        }

        public async Task Logout()
        {
            await _httpService.Post("security/logout");
            await _localStorageService.RemoveItem("accessToken");
            await _localStorageService.RemoveItem("refreshToken");

            ((AuthStateProvidor)_authState).NotifyUserLogout();

            _navigationManager.NavigateTo("login");
        }

       
    }
}

