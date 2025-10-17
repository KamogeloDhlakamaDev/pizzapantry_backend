using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Domain.Response
{

    public record ErrorBody(string Property, string Message, object AttemptedValue);

    public readonly struct OnError
    {
        public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.BadRequest;

        public DateTime RequestMadeAt => DateTime.Now;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Error { get; init; } = default!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IEnumerable<ErrorBody> Errors { get; init; } = default!;

        public OnError(HttpStatusCode statusCode, IEnumerable<ValidationFailure> errors)
        {
            StatusCode = statusCode;
            Errors = errors.Select(e => new ErrorBody(e.PropertyName, e.ErrorMessage, e.AttemptedValue));
        }

        public OnError(HttpStatusCode statusCode, IEnumerable<ValidationFailure> errors, string? error)
        {
            StatusCode = statusCode;
            Error = error;
            Errors = errors.Select(e => new ErrorBody(e.PropertyName, e.ErrorMessage, e.AttemptedValue));
        }

        public OnError(HttpStatusCode statusCode, string? error)
        {
            StatusCode = statusCode;
            Error = error;
        }

        public OnError(IEnumerable<ValidationFailure> errors)
        {
            Errors = errors.Select(e => new ErrorBody(e.PropertyName, e.ErrorMessage, e.AttemptedValue));
        }
    }

}