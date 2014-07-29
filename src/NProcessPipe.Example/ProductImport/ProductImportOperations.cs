using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.ProductImport
{
    public class CreateMissingProductCategories : IOperation<ProductImportRow, ProductImportProcessContext>
    {
        public IEnumerable<ProductImportRow> Execute(ProductImportProcessContext context, IEnumerable<ProductImportRow> data)
        {
            foreach (var row in data)
            {
                if (context.Database.Categories.Count(x => x.Name == row.ProductCategory) == 0)
                {
                    context.Database.Categories.Add(new Category() { Name = row.ProductCategory });
                }

                yield return row;
            }
        }
    }

    public class CreateProduct : IOperation<ProductImportRow, ProductImportProcessContext>,
        IOperationDependsOn<CreateMissingProductCategories>
    {
        public IEnumerable<ProductImportRow> Execute(ProductImportProcessContext context, IEnumerable<ProductImportRow> data)
        {
            foreach (var row in data)
            {
                var matchingCategory = context.Database.Categories.First(x => x.Name == row.ProductCategory);
                var product = new Product(matchingCategory.CategoryGuid) { Name = row.ProductName };

                context.Database.Products.Add(product);

                yield return row;
            }

        }
    }

}
