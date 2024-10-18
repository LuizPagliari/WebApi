using System.ComponentModel.DataAnnotations;

namespace WebApi.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public required string OrderName { get; set; }

        public required string ClientName { get; set; }

        public decimal Price => OrderItem?.Sum(i => i.Item != null ? i.Quantity * i.Item.Price : 0) ?? 0;

        public StatusPedido Status { get; private set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<OrderItem>? OrderItem { get; set; } = new List<OrderItem>();

        public void AdicionarItem(OrderItem item)
        {
            if (Status == StatusPedido.Fechado || Status == StatusPedido.Cancelado)
                throw new InvalidOperationException("Não é possível adicionar itens a um pedido fechado ou cancelado.");

            OrderItem?.Add(item);
        }

        public void RemoverItem(OrderItem item)
        {
            if (Status == StatusPedido.Fechado || Status == StatusPedido.Cancelado)
                throw new InvalidOperationException("Não é possível remover itens de um pedido fechado ou cancelado.");

            OrderItem?.Remove(item);
        }

        public void FecharPedido()
        {
            if (OrderItem == null || !OrderItem.Any())
                throw new InvalidOperationException("Um pedido deve conter ao menos um item para ser fechado.");
            Status = StatusPedido.Fechado;
        }

        public void AtualizarStatus(StatusPedido novoStatus)
        {
            // Remover lógica de fechamento, pois já está verificada em FecharPedido()
            Status = novoStatus;
        }
    }
}
