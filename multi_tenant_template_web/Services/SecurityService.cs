using System.Net.Http;
using System.Configuration;
using CMS.Web.Models;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using System;
using Common.Api.Responses;

namespace CMS.Web.Services
{
    public interface ISecurityService
    {
        Task<IdentifyResult> Identify(); 
    }
    public class SecurityService : ISecurityService
    {
        private IHttpService _httpService;
        private ILocalStorageService _localStorageService;
        private NavigationManager _navigationManager;


        //public  InitTenant { get; private set; }
        public SecurityService(NavigationManager navigationManager,IHttpService httpService, ILocalStorageService localStorageService)
        {
            _httpService = httpService;
            _localStorageService = localStorageService;
            _navigationManager = navigationManager;

        }

        public async Task<IdentifyResult> Identify()
        {
            IdentifyResult response = await _httpService.Get<IdentifyResult>("https://localhost:44381/api/security/identify");

            await _localStorageService.SetItem("TenantLogo", response.Logo);
            return response;
        }
    }
}
