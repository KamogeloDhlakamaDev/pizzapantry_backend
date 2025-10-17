using System.Net;

namespace pizzapantry_backend.Domain.Contracts;

public readonly struct OnSuccess<TResponse> where TResponse : class
{
    public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;

    public DateTime RequestMadeAt => DateTime.Now;

    public TResponse? Response { get; init; } = default!;
    
    public OnSuccess() { }
}
