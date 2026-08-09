using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Models
{
    internal class Product
    {
        public Guid Id { get; private set; }
        public Guid CategoryId { get; private set; }
        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }
        public string Sku { get; private set; } = default!;
        public decimal Price { get; private set; }

        private Product() { }

        public Product Create(
            string name,
            string? description,
            string sku,
            decimal price,
            Guid categoryId)
        {
            return new Product
            {
                Id = Guid.CreateVersion7(),
                Name = name,
                Description = description,
                Sku = sku,
                Price = price,
                CategoryId = categoryId
            };
        }
    }
}
