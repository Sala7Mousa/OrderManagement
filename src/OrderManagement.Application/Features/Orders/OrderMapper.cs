using OrderManagement.Domain;

namespace OrderManagement.Application.Features.Orders;

public static class OrderMapper
{
    public static OrderDto Map(Order order) => new(
        order.Id,
        order.CustomerId,
        order.Status.ToString(),
        order.CreatedAt,
        order.TotalAmount,
        order.Items.Select(item => new OrderItemDto(
            item.ProductId,
            item.Product?.Name ?? string.Empty,
            item.Quantity,
            item.UnitPrice,
            item.LineTotal)).ToList());
}
