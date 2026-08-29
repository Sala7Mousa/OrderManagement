using MediatR;

namespace OrderManagement.Application.Features.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(CreateProductRequest Request) : IRequest<ProductDto>, IAdminRequest;
