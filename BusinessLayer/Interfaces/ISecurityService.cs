using Common.Models;

namespace BusinessLayer.Interfaces
{
    public interface ISecurityService
    {
        public bool AuthenticateUser(string userName, string password, string tenantId);
        public UserClaim GetUserClaim(string userName);
    }
}

