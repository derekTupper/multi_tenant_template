
using BusinessLayer.Interfaces;
using Common.Api.Responses;
using Common.Enums;
using Common.Helpers;
using Common.Models;
using DataLayer;
using DataLayer.Entities.Platform;
using Finbuckle.MultiTenant;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Classes
{
    public class TenantService : ITenantService
    {
        private string _className = "App Service";
        private Logging _logger;
        private PlatformContext _context;

        public TenantService(PlatformContext context, Logging logger)
        {
            _context = context;
            _logger = logger;
        }

        public IdentifyResult IdentifyTenant(string tenantUrl)
        {
            try
            {
                IdentifyResult response = new IdentifyResult();
                var tenant = _context.Tenants.Where(x => x.TenantUrl == tenantUrl).SingleOrDefault();
               
                if (tenant != null)
                {
                    response.TenantFound = true;
                }
                else
                {
                    response.TenantFound = false;
                }

                return response;
            }
            catch(Exception ex)
            {
								throw;
						}
        }
        public void SetupStore(IServiceProvider sp)
        {
            _logger.Log(LogLevels.Info, "Begin Db Transaction - SetupStore" + _className);

            try
            {
                var scopeServices = sp.CreateScope().ServiceProvider;
                var store = scopeServices.GetRequiredService<IMultiTenantStore<TenantModel>>();

                var Tenants = _context.Tenants.Select(row => row);

                foreach (var t in Tenants)
                {
                    store.TryAddAsync(new TenantModel
                    {
                        Id = t.TenantId.ToString(),
                        Identifier = t.TenantUrl,
                        Name = t.TenantName,
                        ConnectionString = t.ConnectionString
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevels.Error, "Error - DbTransaction - SetupStore" + _className, ex);
                throw;
            }
            finally
            {
                _logger.Log(LogLevels.Info, "End Db Transaction - SetupStore" + _className);
            }
        }
    }
}
