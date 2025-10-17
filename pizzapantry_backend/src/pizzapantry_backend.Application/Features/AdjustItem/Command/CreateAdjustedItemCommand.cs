using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Domain.Response;
using pizzapantry_backend.Domain.Common;

namespace pizzapantry_backend.Application.Features.AdjustItem.Command
{
    public record CreateAdjustedItemCommand(AdjustItemRequest AdjustItemRequest) :
    IRequest<Result<OnSuccess<GenericResponse>, OnError>>;


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
                        .GreaterThanOrEqualTo(0)
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