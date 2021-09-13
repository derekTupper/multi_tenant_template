using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Api.Responses
{
    public class IdentifyResult
    {
        public string Logo { get; set; }
        public string  PrivacyPolicy { get; set; }
        public bool TenantFound { get; set; }
    }
}
