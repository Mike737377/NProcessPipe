using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.ProductImport
{
    public class SimpleProductDatabase
    {
        public SimpleProductDatabase()
        {
            Categories = new List<Category>();
            Products = new List<Product>();
        }

        public List<Category> Categories { get; private set; }
        public List<Product> Products { get; private set; }
    }

    public class Category
    {

        public Category()
        {
            CategoryGuid = Guid.NewGuid();
        }

        public Guid CategoryGuid { get; private set; }
        public string Name { get; set; }
    }

    public class Product
    {

        public Product(Guid categoryGuid)
        {
            CategoryGuid = categoryGuid;
        }

        public Guid CategoryGuid { get; private set; }
        public string Name { get; set; }

    }
}
