using DataLayer.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities.Platform
{
		public class User : BaseEntity
    {
        [Key]
        public Guid UserId { get; set; } 
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        [ForeignKey("Tenant")]
        public Guid TenantId { get; set; } 
        public Tenant Tenant { get; set; }
    }
}
