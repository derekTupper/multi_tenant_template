using Common.Api.Responses;
using Common.Enums;
using Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Api.Helpers
{
    public class APIHelper
    {
        private Logging _logger;
        private string _className = "API Helper";
        public APIHelper(Logging logger)
        {
            _logger = logger;
        }
        public static ApiResponse GenApiResponse(int Error, string ErrorData, string Value)
        {
            ApiResponse response = new ApiResponse
            {
                Error = Error,
                ErrorData = ErrorData,
                Value = Value
            };

            return response;
        }
    }
}
