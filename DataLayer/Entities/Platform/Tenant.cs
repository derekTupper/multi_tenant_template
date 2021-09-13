using System;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Platform
{
		public class Tenant
		{
				[Key]
				public Guid TenantId { get; set; }
				public string TenantName { get; set; }
				public string TenantUrl { get; set; }
				public string ConnectionString { get; set; }
		}
}
