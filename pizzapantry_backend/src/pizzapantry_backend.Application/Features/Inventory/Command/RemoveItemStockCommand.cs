using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Domain.Response;
using pizzapantry_backend.Application.Features.AdjustItem.Repository;
using pizzapantry_backend.Application.Features.Inventory.Repository;
using pizzapantry_backend.Domain.Common;

namespace pizzapantry_backend.Application.Features.Inventory.Command
{
    public record RemoveItemStockCommand(string ItemId) :
        IRequest<Result<OnSuccess<GenericResponse>, OnError>>;

    public sealed class RemoveItemStockCommandHandler :
        IRequestHandler<RemoveItemStockCommand, Result<OnSuccess<GenericResponse>, OnError>>

    {
        private readonly ILogger _logger;
        private readonly IInventoryRepository _inventoryRepository;

        public RemoveItemStockCommandHandler(
            ILogger logger,
            IInventoryRepository inventoryRepository
        )
        {
            _logger = logger;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Result<OnSuccess<GenericResponse>, OnError>> Handle(RemoveItemStockCommand request, CancellationToken cancellationToken = default)
        {
            try
            {
                bool isItemRemoved = await _inventoryRepository.RemoveItemStock(request.ItemId);

                if (!isItemRemoved)
                {
                    return new OnError(HttpStatusCode.BadRequest, error: "Could not remove item stock");
                }

                return new OnSuccess<GenericResponse>
                {
                    Response = new GenericResponse()
                    {
                        IsSuccess = true,
                        Message = "Successfully removed the stock",
                        ItemId = request.ItemId
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new OnError(HttpStatusCode.BadRequest, error: "Could not Remove Item Stock.");
            }
        }
    }

    public class RemoveItemStockCommandValidation : AbstractValidator<RemoveItemStockCommand>
    {
        public RemoveItemStockCommandValidation()
        {
            RuleFor(x => x.ItemId)
                .NotEmpty()
                .WithMessage("Cannot have item id.");
        }
    }

}