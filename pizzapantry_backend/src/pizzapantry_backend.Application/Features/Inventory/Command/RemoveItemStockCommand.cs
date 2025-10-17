using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Domain.Response;
using pizzapantry_backend.Domain.Common;

namespace pizzapantry_backend.Application.Features.Inventory.Command
{
    public record RemoveItemStockCommand(string ItemId) :
        IRequest<Result<OnSuccess<GenericResponse>, OnError>>;


}