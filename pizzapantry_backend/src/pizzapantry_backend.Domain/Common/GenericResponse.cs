using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pizzapantry_backend.Domain.Common
{
    public class GenericResponse
    {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public string? ItemId { get; set; }

    }
}