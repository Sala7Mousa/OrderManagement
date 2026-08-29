using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application;
using OrderManagement.Application.Features.Orders.Commands.CancelOrder;
using OrderManagement.Application.Features.Orders.Commands.CreateOrder;
using OrderManagement.Application.Features.Orders.Queries.GetOrder;
using OrderManagement.Application.Features.Orders.Queries.ListOrders;
using OrderManagement.Domain;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/orders")]
public sealed class OrdersController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<OrderDto>>> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        return Ok(await sender.Send(new ListOrdersQuery(page, pageSize), ct));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDto>> Get(Guid id, CancellationToken ct)
    {
        return Ok(await sender.Send(new GetOrderQuery(id), ct));
    }

    [HttpPost]
    [Authorize(Roles = Roles.Customer)]
    public async Task<ActionResult<OrderDto>> Create(CreateOrderRequest request, CancellationToken ct)
    {
        var result = await sender.Send(new CreateOrderCommand(request), ct);
        return Created($"/api/orders/{result.Id}", result);
    }

    [HttpPost("{id:guid}/cancel")]
    [Authorize(Roles = Roles.Customer)]
    public async Task<ActionResult<OrderDto>> Cancel(Guid id, CancellationToken ct)
    {
        return Ok(await sender.Send(new CancelOrderCommand(id), ct));
    }
}
