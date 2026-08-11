using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Modules.Catalog.Models
{
    internal class Category
    {
        public Guid Id { get; private set; }
        public string CategoryName { get; private set; } = default!;
        public Guid? ParentCategoryId { get; private set; }

        private Category() { }

        public static Category Create(
            string categoryName,
            Guid? parentCategoryId = null)
        {
            return new Category
            {
                Id = Guid.CreateVersion7(),
                CategoryName = categoryName,
                ParentCategoryId = parentCategoryId
            };
        }
    }
}
