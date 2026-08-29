using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application;
using OrderManagement.Application.Features.Products.Commands.CreateProduct;
using OrderManagement.Application.Features.Products.Commands.UpdateProduct;
using OrderManagement.Application.Features.Products.Queries.ListProducts;
using OrderManagement.Domain;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/products")]
public sealed class ProductsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<ProductDto>>> List(
        [FromQuery] string? name,
        [FromQuery] string? sku,
        [FromQuery] string sortBy = "name",
        [FromQuery] string sortDirection = "asc",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        return Ok(await sender.Send(
            new ListProductsQuery(name, sku, sortBy, sortDirection, page, pageSize),
            ct));
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<ProductDto>> Create(CreateProductRequest request, CancellationToken ct)
    {
        var result = await sender.Send(new CreateProductCommand(request), ct);
        return Created($"/api/products/{result.Id}", result);
    }

    [HttpPatch("{id:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<ProductDto>> Patch(Guid id, UpdateProductRequest request, CancellationToken ct)
    {
        return Ok(await sender.Send(new UpdateProductCommand(id, request), ct));
    }
}
