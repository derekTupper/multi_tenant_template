using BusinessLayer.Interfaces;
using Common.Helpers;
using Common.Models;
using DataLayer;
using DataLayer.Entities.Tenant;
using System.Linq;

namespace BusinessLayer.Classes
{
		public class SecurityService : ISecurityService
    {
        private string _className = "Security Service";
        private TenantContext _tenantCtx;
        private PlatformContext _platformCtx;
        private Logging _logger;

        public SecurityService(PlatformContext platformCtx, Logging logger, TenantContext tenantCtx)
        {
            _tenantCtx = tenantCtx;
            _platformCtx = platformCtx;
            _logger = logger;
        }

        public bool AuthenticateUser(string userName, string password, string tenantId)
        {
            try
            {
                var user = _platformCtx.User.Where(x => x.TenantId.ToString() == tenantId).SingleOrDefault();

                if(user == null || !PasswordValidator(password, user.PasswordHash))
                {
                    return false;
                }
                else
                {
                    return true;
                }    
            }
            catch
            {
                return false;
            }
        }

        public UserClaim GetUserClaim(string userName)
        {
            try
            {
                var user = _platformCtx.User.SingleOrDefault(x => x.UserName == userName);
                var tenantInfo = _platformCtx.Tenants.SingleOrDefault(x => x.TenantId == user.TenantId);

                return new UserClaim
                {
                    TenantId = tenantInfo.TenantId.ToString(),
                };
            }
            catch
            {
                return null;
            }
        }

        private bool PasswordValidator(string enteredpassword, string storedpassword)
        {
            if (enteredpassword == storedpassword)
                return true;
            else
                return false;

        }
    }
}
