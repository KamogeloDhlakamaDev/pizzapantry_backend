using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pizzapantry_backend.Domain.Common
{
    public class ApiError
    {
        public string? Message { get; set; }

        public Dictionary<string, string[]>? ValidationErrors { get; set; }
        public int StatusCode { get; set; }
    }
}