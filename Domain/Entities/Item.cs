using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Domain.Entities
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Quantity { get; set; } = 0;

        [DataType(DataType.Currency)]
        public decimal Price { get; set; } = 0;

        public string? Description { get; set; }

        public string? Category { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<OrderItem>? OrderProducts { get; set; }
    }
}