namespace WebApi.Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }

        public enum OrderStatus
        {
            Aberto,
            Fechado,
            Cancelado
        }

        public string OrderName { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        public OrderStatus Status { get; set; }

    }
}