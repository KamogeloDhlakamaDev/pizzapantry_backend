using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using pizzapantry_backend.Application.Features.Inventory.Command;
using pizzapantry_backend.Domain.Common;

namespace pizzapantry_backend.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ISoftiator _softiator;
        public InventoryController(ISoftiator softiator)
        {
            _softiator = softiator;
        }

        [HttpPost("create_inventory_item")]
        public async Task<IActionResult> CreateInventoryItem([FromBody] CreateItemRequest createItemRequest)
        {
            try
            {
                var command = new CreateItemCommand(createItemRequest);
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