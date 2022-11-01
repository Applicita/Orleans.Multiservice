using System.Collections.Immutable;

namespace Applicita.eShop.CatalogService;

sealed class CatalogGrain : Grain, ICatalogGrain
{
    [GenerateSerializer]
    internal sealed class Catalog
    {
        [Id(0)] public int NewProductId { get; set; } = 0;
        [Id(1)] public List<Product> Products { get; set; } = new();
    }

    readonly IPersistentState<Catalog> catalog;

    public CatalogGrain([PersistentState("state")] IPersistentState<Catalog> catalog) => this.catalog = catalog;

    public async Task<int> CreateProduct(Product product)
    {
        int newProductId = catalog.State.NewProductId++;
        catalog.State.Products.Add(product with { Id = newProductId });
        await catalog.WriteStateAsync();
        return newProductId;
    }

    public Task<ImmutableArray<Product>> GetAllProducts() => Task.FromResult(ImmutableArray.CreateRange(catalog.State.Products));

    public async Task UpdateProduct(Product product)
    {
        var products = catalog.State.Products;
        for (int i = 0; i < products.Count; i++)
        {
            if (products[i].Id == product.Id)
            {
                products[i] = product;
                await catalog.WriteStateAsync();
                return;
            }
        }
        throw new ArgumentException($"Product id {product.Id} not found");
    }

    public async Task DeleteProduct(int id)
    {
        var products = catalog.State.Products;
        for (int i = 0; i < products.Count; i++)
        {
            if (products[i].Id == id)
            {
                products.RemoveAt(i);
                await catalog.WriteStateAsync();
                return;
            }
        }
        throw new ArgumentException($"Product id {id} not found");
    }
}
