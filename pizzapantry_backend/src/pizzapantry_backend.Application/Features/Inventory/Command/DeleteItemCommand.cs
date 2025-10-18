using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Domain.Response;
using pizzapantry_backend.Application.Features.Inventory.Repository;
using pizzapantry_backend.Domain.Common;
using SharpCompress.Archives;

namespace pizzapantry_backend.Application.Features.Inventory.Command
{
    public record DeleteItemCommand(string ItemId) :
        IRequest<Result<OnSuccess<GenericResponse>, OnError>>;

    public sealed class DeleteItemCommandHandler :
        IRequestHandler<DeleteItemCommand, Result<OnSuccess<GenericResponse>, OnError>>

    {
        private readonly ILogger _logger;
        private readonly IInventoryRepository _inventoryRepository;

        public DeleteItemCommandHandler(
            ILogger logger,
            IInventoryRepository inventoryRepository
        )
        {
            _logger = logger;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Result<OnSuccess<GenericResponse>, OnError>> Handle(DeleteItemCommand request, CancellationToken cancellationToken = default)
        {
            try
            {
                bool isItemRemoved = await _inventoryRepository.DeleteItem(request.ItemId);

                if (!isItemRemoved)
                {
                    return new OnError(HttpStatusCode.BadRequest, error: "Could not remove item stock");
                }

                return new OnSuccess<GenericResponse>
                {
                    Response = new GenericResponse()
                    {
                        IsSuccess = true,
                        Message = "Successfully Deleted item",
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

    public class DeleteItemCommandValidation : AbstractValidator<DeleteItemCommand>
    {
        public DeleteItemCommandValidation()
        {
            RuleFor(x => x.ItemId)
                .NotEmpty()
                .WithMessage("Cannot have item id.");
        }
    }


}