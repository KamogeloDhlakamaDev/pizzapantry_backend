using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using pizzapantry_backend.Application.Features.Inventory.Command;
using pizzapantry_backend.Application.Features.Inventory.Query;
using pizzapantry_backend.Domain.Common;

namespace pizzapantry_backend.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
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

        [HttpPost("update_inventory_item")]
        public async Task<IActionResult> UpdateInventoryItem([FromBody] UpdateItemRequest createItemRequest)
        {
            try
            {
                var command = new UpdateItemCommand(createItemRequest);
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

        [HttpPost("remove_stock/{itemId}")]
        public async Task<IActionResult> RemoveItemStock(string itemId)
        {
            try
            {
                var command = new RemoveItemStockCommand(itemId);
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

        [HttpGet("get_all_inventory_items")]
        public async Task<IActionResult> GetInventoryAllItems()
        {
            try
            {
                var command = new GetInventoryItemsQuery();
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

        [HttpGet("get_inventory_item_info/{itemId}")]
        public async Task<IActionResult> GetInventoryItemInfo(string itemId)
        {
            try
            {
                var command = new GetInventoryItemInfoQuery(itemId);
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

        [HttpGet("get_detailed_inventory_item_info/{itemId}")]
        public async Task<IActionResult> GetDetailedInventoryItemInfo(string itemId)
        {
            try
            {
                var command = new GetInventoryItemDetailedInfoQuery(itemId);
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