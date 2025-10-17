using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pizzapantry_backend.Application.Features.AdjustItem.Query;
using pizzapantry_backend.Domain.Mongo;

namespace pizzapantry_backend.Application.Features.AdjustItem.Repository
{
    public interface IAdjustItemRespositry
    {
        Task<bool> AdjustItemQuanty(AdjustmentHistory adjustItem);

        Task<ItemToAdjustDto?> GetItemToAdjust(string ItemId);
    }
}