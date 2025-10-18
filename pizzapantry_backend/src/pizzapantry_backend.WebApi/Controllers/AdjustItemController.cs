using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using pizzapantry_backend.Application.Features.AdjustItem.Command;
using pizzapantry_backend.Application.Features.AdjustItem.Query;
using pizzapantry_backend.Domain.Common;

namespace pizzapantry_backend.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]

    public class AdjustItemController : ControllerBase
    {
        private readonly ISoftiator _softiator;
        public AdjustItemController(ISoftiator softiator)
        {
            _softiator = softiator;
        }

        [HttpPost("create_adjusted_item_quantity")]
        public async Task<IActionResult> UpdateInventoryItem([FromBody] AdjustItemRequest adjustItemRequest)
        {
            try
            {
                var command = new CreateAdjustedItemCommand(adjustItemRequest);
                var result = await _softiator.Send(command);

                return result.Match<IActionResult>(
                    success => Ok(success.Response),
                    failure => StatusCode((int)failure.StatusCode, new ApiError
                    {
                        Message = failure.Error,
                        StatusCode = (int)failure.StatusCode
                    })
                );
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(new ApiError
                {
                    Message = "Validation failed.",
                    ValidationErrors = errors,
                    StatusCode = 400
                });
            }
        }


        [HttpGet("get_item_to_adjust/{itemId}")]
        public async Task<IActionResult> GetAdjustItemInfo(string itemId)
        {
            try
            {
                var command = new GetItemToAdjustInfoQuery(itemId);
                var result = await _softiator.Send(command);

                return result.Match<IActionResult>(
                    success => Ok(success.Response),
                    failure => StatusCode((int)failure.StatusCode, new ApiError
                    {
                        Message = failure.Error,
                        StatusCode = (int)failure.StatusCode
                    })
                );
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(new ApiError
                {
                    Message = "Validation failed.",
                    ValidationErrors = errors,
                    StatusCode = 400
                });
            }
        }
    }
}