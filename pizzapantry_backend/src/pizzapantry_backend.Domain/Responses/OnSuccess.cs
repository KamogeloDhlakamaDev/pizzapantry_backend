using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Domain.Response
{
    public readonly struct OnSuccess<TResponse> where TResponse : class
    {
        public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;

        public DateTime RequestMadeAt => DateTime.Now;

        public TResponse? Response { get; init; } = default!;

        public OnSuccess() { }
    }
}