# NProcessPipe #
----------

NProcessPipe is a simple framework which can be used in advanced scenarios where you need to run data through a pipeline or workflow.

## How to Install ##

NProcessPipe is available on NuGet at [https://www.nuget.org/packages/NProcessPipe/](https://www.nuget.org/packages/NProcessPipe/)

You can download and install through the NuGet Package Manager GUI or through NuGet Package Console with the command 

> Install-Package NProcessPipe

## Getting Started Example ##

In this example I'll take you through creating a process to import products into a database.

### Background. ###
Here's our database model which we want to store the data in.

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

### Step 1. ###
Create your data model which you want to process. Let's pretend we have extracted this data from a CSV file and are storing it in this object for the meantime.

    public class ProductImportRow
    {
    	public string ProductName { get; set; }
    	public string ProductCategory { get; set; }
    }
	
### Step 2. ###
Create your context. The context is where we can keep things that are not row specific, such as the database or a place where we can log messages to.	
	
	
    public class ProductImportProcessContext : IProcessContext
    {

        public ProductImportProcessContext(IProcessLogger log, IDictionary<string, dynamic> contextData)
        {
            Log = log;
            Database = contextData["database"];
        }

        public IProcessLogger Log { get; private set; }
        public SimpleProductDatabase Database { get; private set; }

    }

### Step 3. ###
Create your process. The process is what will create our context and assign our logger as well as execute our operations in the next step.

    public class ProductImportProcess : Process<ProductImportRow, ProductImportProcessContext>
    {
        protected override IProcessLogger CreateLog()
        {
            return new ConsoleLog();
        }

        protected override ProductImportProcessContext CreateProcessContext(IProcessLogger log, IDictionary<string, dynamic> contextData)
        {
            return new ProductImportProcessContext(log, contextData);
        }
    }

### Step 4.###
Create your operations. Each operation allows us to perform a unit of work whilst enumerating through our data. We also have access to our context whilst we are processing our data.

In our particular scenario we want to create our distinct categories and then match up our products to their categories and save it all to our database. Note that the CreateProduct operation depends on the CreateMissingProductCategories operation, this is how we create dependencies. You can have 0..n dependencies for each operation. Order will automatically be determined at runtime.

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

### Step 5.###
Wire it all together via executing the process over our data.

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


**If you require more examples, take a look at the examples project in the 'src' folder.**