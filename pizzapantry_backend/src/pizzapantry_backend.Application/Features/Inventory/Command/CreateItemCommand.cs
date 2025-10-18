using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Domain.Response;
using Microsoft.AspNetCore.RateLimiting;
using pizzapantry_backend.Application.Features.Inventory.Repository;
using pizzapantry_backend.Domain.Common;
using pizzapantry_backend.Domain.Mongo;

namespace pizzapantry_backend.Application.Features.Inventory.Command
{
    public record CreateItemCommand(CreateItemRequest CreateItemRequest) :
        IRequest<Result<OnSuccess<GenericResponse>, OnError>>;

    public sealed class CreateItemCommandHandler :
        IRequestHandler<CreateItemCommand, Result<OnSuccess<GenericResponse>, OnError>>
    {
        private ILogger _logger;
        private IInventoryRepository _inventoryRepository;

        public CreateItemCommandHandler(
            ILogger logger,
            IInventoryRepository inventoryRepository
        )
        {
            _logger = logger;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Result<OnSuccess<GenericResponse>, OnError>> Handle(CreateItemCommand request, CancellationToken cancellationToken = default)
        {
            try
            {

                Item createItem = new Item
                {
                    Category = request.CreateItemRequest.Category,
                    CreatedOn = DateTime.Now,
                    CurrentQuanity = request.CreateItemRequest.CurrentQuanity,
                    Description = request.CreateItemRequest.Description,
                    ItemName = request.CreateItemRequest.ItemName,
                    Location = request.CreateItemRequest.Location,
                    MinimumQuantity = request.CreateItemRequest.MinimumQuantity,

                    SellingPrice = request.CreateItemRequest.SellingPrice,
                    SKU = request.CreateItemRequest.SKU
                };

                bool isCreated = await _inventoryRepository.CreateItem(createItem);

                return new OnSuccess<GenericResponse>
                {
                    Response = new GenericResponse
                    {
                        IsSuccess = true,
                        Message = "Successfully created an item. "
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

    public sealed class CreateItemCommandValidation : AbstractValidator<CreateItemCommand>
    {
        public CreateItemCommandValidation()
        {
            RuleFor(x => x.CreateItemRequest)
            .NotNull().WithMessage("CreateItemRequest is required.");

            When(x => x.CreateItemRequest != null, () =>
            {
                RuleFor(x => x.CreateItemRequest.ItemName)
                    .NotEmpty().WithMessage("ItemName is required.")
                    .MaximumLength(200).WithMessage("ItemName must be at most 200 characters.");

                RuleFor(x => x.CreateItemRequest.SKU)
                    .MaximumLength(100).WithMessage("SKU must be at most 100 characters.");

                RuleFor(x => x.CreateItemRequest.Category)
                    .MaximumLength(100).WithMessage("Category must be at most 100 characters.");

                RuleFor(x => x.CreateItemRequest.CurrentQuanity)
                    .GreaterThanOrEqualTo(0).WithMessage("Current Quanity must be 0 or greater.");

                RuleFor(x => x.CreateItemRequest.Location)
                    .MaximumLength(200).WithMessage("Location must be at most 200 characters.");

                RuleFor(x => x.CreateItemRequest.Description)
                    .MaximumLength(1000).WithMessage("Description must be at most 1000 characters.");

            });
        }
    }

    public class CreateItemRequest
    {
        public required string ItemName { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int CurrentQuanity { get; set; }
        public int MinimumQuantity { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double SellingPrice { get; set; }

    }
}