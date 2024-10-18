using WebApi.Domain.Entities;
using WebApi.Domain.Repositories;

namespace WebApi.Domain.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // Regra para fechar o pedido
        public async Task<bool> FecharPedidoAsync(int orderId)
        {
            // Obtém o pedido pelo ID
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new InvalidOperationException("Pedido não encontrado.");

            // Fecha o pedido usando a lógica da entidade
            order.FecharPedido();
            await _orderRepository.UpdateAsync(order);

            return true;
        }

        // Regra para adicionar um item ao pedido
        public async Task AdicionarItemAoPedidoAsync(int orderId, OrderItem item)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new InvalidOperationException("Pedido não encontrado.");

            order.AdicionarItem(item);
            await _orderRepository.UpdateAsync(order);
        }

        // Regra para remover um item do pedido
        public async Task RemoverItemDoPedidoAsync(int orderId, OrderItem item)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new InvalidOperationException("Pedido não encontrado.");

            order.RemoverItem(item);
            await _orderRepository.UpdateAsync(order);
        }
    }
}
