using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Domain.Response;
using Microsoft.EntityFrameworkCore.Update.Internal;
using pizzapantry_backend.Application.Features.Inventory.Repository;
using pizzapantry_backend.Domain.Common;
using pizzapantry_backend.Domain.Mongo;

namespace pizzapantry_backend.Application.Features.Inventory.Command
{
    public record UpdateItemCommand(UpdateItemRequest UpdateItemRequest) :
        IRequest<Result<OnSuccess<GenericResponse>, OnError>>;

    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, Result<OnSuccess<GenericResponse>, OnError>>
    {
        private readonly ILogger _logger;
        private readonly IInventoryRepository _inventoryRepository;

        public UpdateItemCommandHandler(ILogger logger, IInventoryRepository inventoryRepository)
        {
            _logger = logger;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Result<OnSuccess<GenericResponse>, OnError>> Handle(UpdateItemCommand request, CancellationToken cancellationToken = default)
        {
            try
            {

                Item? updateItem = await _inventoryRepository.GetInventoryItem(request.UpdateItemRequest.ItemId);

                if (updateItem is null)
                {
                    return new OnError(HttpStatusCode.NotFound, error: "Could not find associated item");
                }
                updateItem.Category = request.UpdateItemRequest.Category;
                updateItem.CurrentQuanity = request.UpdateItemRequest.Quanity;
                updateItem.Description = request.UpdateItemRequest.Description;
                updateItem.ItemName = request.UpdateItemRequest.ItemName;
                updateItem.Location = request.UpdateItemRequest.Location;
                updateItem.MinimumQuantity = request.UpdateItemRequest.MinimumQuantity;
                updateItem.SellingPrice = request.UpdateItemRequest.SellingPrice;
                updateItem.SKU = request.UpdateItemRequest.SKU;

                bool isUpdated = await _inventoryRepository.UpdateItem(updateItem);

                if (!isUpdated)
                    return new OnError(HttpStatusCode.BadRequest, error: "Could not update the item.");

                return new OnSuccess<GenericResponse>
                {
                    Response = new GenericResponse
                    {
                        IsSuccess = true,
                        Message = "Successfully updated an item.",
                        ItemId = updateItem.ItemId.ToString()
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new OnError(HttpStatusCode.BadRequest, error: "Could not create inventory item.");
            }
        }
    }

    public sealed class UpdateItemCommandValidation : AbstractValidator<UpdateItemCommand>
    {
        public UpdateItemCommandValidation()
        {
            RuleFor(x => x.UpdateItemRequest)
            .NotNull().WithMessage("UpdateItemRequest is required.");

            When(x => x.UpdateItemRequest != null, () =>
            {
                RuleFor(x => x.UpdateItemRequest.ItemName)
                    .NotEmpty().WithMessage("ItemName is required.")
                    .MaximumLength(200).WithMessage("ItemName must be at most 200 characters.");

                RuleFor(x => x.UpdateItemRequest.SKU)
                    .MaximumLength(100).WithMessage("SKU must be at most 100 characters.");

                RuleFor(x => x.UpdateItemRequest.Category)
                    .MaximumLength(100).WithMessage("Category must be at most 100 characters.");

                RuleFor(x => x.UpdateItemRequest.Quanity)
                    .GreaterThanOrEqualTo(0).WithMessage("Current Quanity must be 0 or greater.");

                RuleFor(x => x.UpdateItemRequest.Location)
                    .MaximumLength(200).WithMessage("Location must be at most 200 characters.");

                RuleFor(x => x.UpdateItemRequest.Description)
                    .MaximumLength(1000).WithMessage("Description must be at most 1000 characters.");

            });
        }
    }

    public class UpdateItemRequest
    {
        public required string ItemId { get; set; }
        public required string ItemName { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Quanity { get; set; }
        public int MinimumQuantity { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double SellingPrice { get; set; }
    }
}