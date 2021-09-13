using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Api.Responses
{
    public class ApiResponse
    {
        public int Error { get; set; }
        public string ErrorData { get; set; }
        public string Value { get; set; }
    }
}
