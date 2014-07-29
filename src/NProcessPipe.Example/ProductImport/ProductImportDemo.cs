using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProcessPipe.Example.ProductImport
{
    public class ProductImportDemo
    {

        public void Main()
        {
            //setup the data
            var data = new ProductImportRow[]
            {
                new ProductImportRow() { ProductCategory = "Toys" , ProductName = "Children's Blocks" },
                new ProductImportRow() { ProductCategory = "Electronics" , ProductName = "Mobile Phone" },
                new ProductImportRow() { ProductCategory = "Electronics" , ProductName = "Gaming Console" },
                new ProductImportRow() { ProductCategory = "Electronics" , ProductName = "TV" },
                new ProductImportRow() { ProductCategory = "Toys" , ProductName = "Board Game" }
            };

            //create the database and pass it into the context via the context data
            var database = new SimpleProductDatabase();
            var contextData = new Dictionary<string, dynamic>();
            contextData.Add("database", database);

            //let's run our process over our data
            var process = new ProductImportProcess();
            process.Execute(data, contextData);

            //project our data from the database 
            var savedData = from p in database.Products
                            join c in database.Categories on p.CategoryGuid equals c.CategoryGuid
                            select string.Format("{0}, {1}, {2}", c.CategoryGuid, c.Name, p.Name);

            //display our results
            Console.WriteLine("Category Guid, Category, Product");
            foreach (var item in savedData)
            {
                Console.WriteLine(item);
            }
        }

    }
}
