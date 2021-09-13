using BusinessLayer.Interfaces;
using Common.Api.Requests;
using Common.Api.Responses;
using Common.Enums;
using Common.Helpers;
using Common.Models;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CMS.Api.Controllers
{
    [Route("api/security")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private string _className = "Security Controller";
        private readonly ITenantService _appService;
        private readonly ISecurityService _security;
        private readonly ITokenManager _jwtManager;
        private readonly IMultiTenantStore<TenantModel> _store;
        private readonly string _tenantId;
        private Logging _logger;
        public SecurityController(ISecurityService security, ITokenManager jwtManager, Logging logging, ITenantService appService, IMultiTenantStore<TenantModel> store)
        {
            _security = security;
            _jwtManager = jwtManager;
            _logger = logging;
            _appService = appService;
            _store = store;

        }
        [AllowAnonymous]
        [HttpGet("store")]
        public ActionResult Index()
        {
            var ti = Task.FromResult(_store.GetAllAsync());
            return Ok(ti);
        }


        [AllowAnonymous]
        [HttpGet("identify")]
        public ActionResult IdentifyTenant()
      {
            _logger.Log(LogLevels.Info, "Begin Request - Identify Tenant" + _className);
            try
            {
                string host = HttpContext.Request.Headers["__tenant__"];
                IdentifyResult response = _appService.IdentifyTenant(host);
             
                if (response == null)
                {
                    return NotFound();
                }

                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevels.Error, "Error Request - Identity Tenant" + _className, ex);
                return BadRequest();
            }
            finally
            {
                _logger.Log(LogLevels.Info, "End Request - Identify Tenant" + _className);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            _logger.Log(LogLevels.Info, "Begin Request - Login" + _className);
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (!_security.AuthenticateUser(request.Username, request.Password, _tenantId))
                {
                    return Unauthorized();
                }

                UserClaim customClaim = _security.GetUserClaim(request.Username);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Username),
                    new Claim("TenantID", customClaim.TenantId),
                    new Claim("ApplicationID", customClaim.ApplicationId)
                };

                var jwtResult = _jwtManager.GenerateTokens(request.Username, claims, DateTime.Now);

                return Ok(new LoginResult
                {
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                });
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevels.Error, "Error Request - Login" + _className, ex);
                return Unauthorized();
            }
            finally
            {
                _logger.Log(LogLevels.Info, "End Request - Login" + _className);
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            var userName = User.Identity.Name;
            _jwtManager.RemoveRefreshTokenByUserName(userName);
            return Ok();
        }

        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            _logger.Log(LogLevels.Info, "Begin Request - Refresh Token" + _className);
            try
            {
                var userName = User.Identity.Name;

                if (string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    return Unauthorized();
                }

                var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
                var jwtResult = _jwtManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
                return Ok(new LoginResult
                {
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                });
            }
            catch (SecurityTokenException ex)
            {
                _logger.Log(LogLevels.Error, "Error Request - Refresh Token" + _className, ex);
                return Unauthorized(ex.Message);
            }
            finally
            {
                _logger.Log(LogLevels.Info, "End Request - Refresh Token" + _className);
            }
        }
    }
}
