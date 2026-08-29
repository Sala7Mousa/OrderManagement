using MediatR;

namespace OrderManagement.Application.Features.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(Guid Id, UpdateProductRequest Request) : IRequest<ProductDto>, IAdminRequest;
