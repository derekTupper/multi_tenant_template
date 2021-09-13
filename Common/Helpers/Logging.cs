using log4net;
using Common.Enums;
using System;

namespace Common.Helpers
{
    public class Logging
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Guid _tenantId;
    
        
        public Logging()
        {
        }

        public void Log(LogLevels level, string message, System.Exception exception)
        {

            message = String.Format("TenantId: {0} -- {1}", _tenantId, message);

            switch (level)
            {
                case LogLevels.Debug:
                    log.Debug(message, exception);
                    break;
                case LogLevels.Error:
                    log.Error(message, exception);
                    break;
                case LogLevels.Info:
                    log.Info(message);
                    break;
                case LogLevels.Warn:
                    log.Info(message, exception);
                    break;
                case LogLevels.Fatal:
                    log.Info(message, exception);
                    break;
            }
        }


        public void Log(LogLevels level, string message)
        {

            message = String.Format("TenantId: {0} -- {1}", _tenantId, message);

            switch (level)
            {
                case LogLevels.Debug:
                    log.Debug(message);
                    break;
                case LogLevels.Error:
                    log.Error(message);
                    break;
                case LogLevels.Info:
                    log.Info(message);
                    break;
                case LogLevels.Warn:
                    log.Info(message);
                    break;
                case LogLevels.Fatal:
                    log.Info(message);
                    break;
            }

        }
    }
}
