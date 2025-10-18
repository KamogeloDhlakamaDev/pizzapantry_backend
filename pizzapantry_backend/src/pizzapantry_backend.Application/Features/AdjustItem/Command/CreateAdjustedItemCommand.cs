using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Domain.Response;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pizzapantry_backend.Application.Features.AdjustItem.Repository;
using pizzapantry_backend.Domain.Common;
using pizzapantry_backend.Domain.Mongo;

namespace pizzapantry_backend.Application.Features.AdjustItem.Command
{
    public record CreateAdjustedItemCommand(AdjustItemRequest AdjustItemRequest) :
    IRequest<Result<OnSuccess<GenericResponse>, OnError>>;

    public class CreateAdjustedItemCommandHandler :
        IRequestHandler<CreateAdjustedItemCommand, Result<OnSuccess<GenericResponse>, OnError>>
    {
        private readonly ILogger _logger;
        private readonly IAdjustItemRespositry _adjustItemRepository;

        public CreateAdjustedItemCommandHandler(
            ILogger logger,
            IAdjustItemRespositry adjustItemRespositry
        )
        {
            _logger = logger;
            _adjustItemRepository = adjustItemRespositry;
        }

        public async Task<Result<OnSuccess<GenericResponse>, OnError>> Handle(CreateAdjustedItemCommand request, CancellationToken cancellationToken = default)
        {
            try
            {

                AdjustmentHistory adjustmentHistory = new AdjustmentHistory()
                {
                    CreatedOn = DateTime.Now,
                    ItemId = request.AdjustItemRequest.ItemId,
                    CreatedBy = "User", //TODO:Change 
                    Quantity = request.AdjustItemRequest.Quanity,
                    Reason = request.AdjustItemRequest.Reason

                };
                bool isCreated = await _adjustItemRepository.AdjustItemQuanty(adjustmentHistory);

                if (!isCreated)
                {
                    return new OnError(HttpStatusCode.BadRequest, error: "Could not create adjusted item history");
                }

                return new OnSuccess<GenericResponse>
                {
                    StatusCode = HttpStatusCode.OK,
                    Response = new GenericResponse()
                    {
                        IsSuccess = true,
                        ItemId = request.AdjustItemRequest.ItemId,
                        Message = "Successfully created an item history."
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new OnError(HttpStatusCode.BadRequest, error: "Could not get create adjusted item history.");
            }
        }
    }


    public class CreateAdjustedItemCommandValidation : AbstractValidator<CreateAdjustedItemCommand>
    {
        public CreateAdjustedItemCommandValidation()
        {
            {
                RuleFor(x => x.AdjustItemRequest)
                    .NotNull()
                    .WithMessage("AdjustItemRequest is required.");

                When(x => x.AdjustItemRequest != null, () =>
                {
                    RuleFor(x => x.AdjustItemRequest.Quanity)
                        .NotEmpty()
                        .WithMessage("Quanity must be greater than or equal to 0.");

                    RuleFor(x => x.AdjustItemRequest.ItemId)
                        .NotEmpty()
                        .WithMessage("ItemId is required.");

                    RuleFor(x => x.AdjustItemRequest.Reason)
                        .NotEmpty()
                        .WithMessage("Reason is required.")
                        .MaximumLength(1000)
                        .WithMessage("Reason must be at most 1000 characters.");
                });
            }
        }
    }

    public class AdjustItemRequest
    {
        public int Quanity { get; set; }
        public required string ItemId { get; set; }
        public required string Reason { get; set; }
    }
}