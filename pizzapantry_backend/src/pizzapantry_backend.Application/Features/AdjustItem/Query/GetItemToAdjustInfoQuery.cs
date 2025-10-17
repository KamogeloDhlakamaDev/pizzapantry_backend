using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Domain.Response;
using pizzapantry_backend.Application.Features.AdjustItem.Repository;

namespace pizzapantry_backend.Application.Features.AdjustItem.Query
{
    public record GetItemToAdjustInfoQuery(string ItemId) :
        IRequest<Result<OnSuccess<ItemToAdjustResponse>, OnError>>;

    public sealed class GetItemToAdjustInfoQueryHandler :
        IRequestHandler<GetItemToAdjustInfoQuery, Result<OnSuccess<ItemToAdjustResponse>, OnError>>
    {
        private readonly ILogger _logger;
        private readonly IAdjustItemRespositry _adjustItemRepository;

        public GetItemToAdjustInfoQueryHandler(
            ILogger logger,
            IAdjustItemRespositry adjustItemRespositry
        )
        {
            _logger = logger;
            _adjustItemRepository = adjustItemRespositry;
        }

        public async Task<Result<OnSuccess<ItemToAdjustResponse>, OnError>> Handle(GetItemToAdjustInfoQuery request, CancellationToken cancellationToken = default)
        {
            try
            {
                ItemToAdjustDto? itemToAdjustDto = await _adjustItemRepository.GetItemToAdjust(request.ItemId);

                if (itemToAdjustDto is null)
                {
                    return new OnError(HttpStatusCode.NotFound, error: "Could not find item associated with the item id");
                }

                return new OnSuccess<ItemToAdjustResponse>
                {
                    StatusCode = HttpStatusCode.OK,
                    Response = new ItemToAdjustResponse()
                    {
                        ItemToAdjust = itemToAdjustDto
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new OnError(HttpStatusCode.BadRequest, error: "Could not get Item to Adjust info.");
            }
        }
    }

    public sealed class GetItemToAdjustInfoQueryValidation : AbstractValidator<GetItemToAdjustInfoQuery>
    {
        public GetItemToAdjustInfoQueryValidation()
        {
            RuleFor(x => x.ItemId)
                .NotEmpty()
                .WithMessage("Item Id is empty.")
                .NotNull()
                .WithMessage("Item Id is not null");
        }
    }

    public class ItemToAdjustResponse
    {
        public ItemToAdjustDto? ItemToAdjust { get; set; }
    }

    public class ItemToAdjustDto
    {
        public required string ItemId { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public int MinimumStock { get; set; }
    }
}