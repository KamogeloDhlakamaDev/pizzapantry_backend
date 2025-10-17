using Application.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Softiator
{
    public class Softiator : ISoftiator
    {
        private readonly IServiceProvider _serviceProvider;

        public Softiator (IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Send<TResponse> (IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var requestType = request.GetType();
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));

            var handler = _serviceProvider.GetRequiredService(handlerType);
            var method = handlerType.GetMethod("Handle");

            if (method is null)
                throw new InvalidOperationException($"Handler does not implement a 'Handle' method: {handlerType.FullName}");

            var pipelineType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
            var behaviors = _serviceProvider.GetServices(pipelineType).Cast<object>().Reverse().ToList();

            Func<Task<TResponse>> handlerDelegate = () =>
            {
                var result = method.Invoke(handler, new object [] { request, cancellationToken });

                if (result is not Task<TResponse> typedTask)
                    throw new InvalidOperationException($"Handle method did not return Task<{typeof(TResponse).Name}>");

                return typedTask;
            };

            foreach (var behavior in behaviors)
            {
                var current = handlerDelegate;
                var behaviorHandle = pipelineType.GetMethod("Handle");

                if (behaviorHandle is null)
                    throw new InvalidOperationException($"Behavior does not implement 'Handle' method: {pipelineType.FullName}");

                handlerDelegate = () =>
                {
                    var result = behaviorHandle.Invoke(behavior, new object [] { request, cancellationToken, current });

                    if (result is not Task<TResponse> typedTask)
                        throw new InvalidOperationException($"Behavior Handle method did not return Task<{typeof(TResponse).Name}>");

                    return typedTask;
                };
            }

            return await handlerDelegate();
        }
    }


}