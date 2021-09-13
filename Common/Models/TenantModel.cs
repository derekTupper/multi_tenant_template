 using Finbuckle.MultiTenant;

namespace Common.Models
{
    public class TenantModel : ITenantInfo
    {
        public string Id { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string Logo { get; set; }
        public string PrivacyPolicy { get; set; }
    }
}
