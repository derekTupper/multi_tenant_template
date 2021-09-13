using Common.Api.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ITenantService
    {
        public IdentifyResult IdentifyTenant(string tenantUrl);
        public void SetupStore(IServiceProvider sp);
    }
}

