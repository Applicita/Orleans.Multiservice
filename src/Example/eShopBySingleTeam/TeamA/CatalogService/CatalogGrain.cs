using System.Collections.Immutable;

namespace Applicita.eShop.CatalogService;

sealed class CatalogGrain([PersistentState("state")] IPersistentState<CatalogGrain.Catalog> catalog) : Grain, ICatalogGrain
{
    [GenerateSerializer]
    internal sealed class Catalog
    {
        [Id(0)] public int NewProductId { get; set; } = 0;
        [Id(1)] public List<Product> Products { get; set; } = [];
    }

    public async Task<int> CreateProduct(Product product)
    {
        int newProductId = catalog.State.NewProductId++;
        catalog.State.Products.Add(product with { Id = newProductId });
        await catalog.WriteStateAsync();
        return newProductId;
    }

    public Task<ImmutableArray<Product>> GetAllProducts() => Task.FromResult(ImmutableArray.CreateRange(catalog.State.Products));

    public Task<ImmutableArray<Product>> GetCurrentProducts(ImmutableArray<int> productIds)
    {
        var products = catalog.State.Products;
        var currentProducts = productIds.Select(id => products.FirstOrDefault(p => p.Id == id)).OfType<Product>();
        return Task.FromResult(currentProducts.ToImmutableArray());
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var products = catalog.State.Products;
        for (int i = 0; i < products.Count; i++)
        {
            if (products[i].Id == product.Id)
            {
                products[i] = product;
                await catalog.WriteStateAsync();
                return true;
            }
        }
        return false;
    }

    public async Task<bool> DeleteProduct(int id)
    {
        var products = catalog.State.Products;
        for (int i = 0; i < products.Count; i++)
        {
            if (products[i].Id == id)
            {
                products.RemoveAt(i);
                await catalog.WriteStateAsync();
                return true;
            }
        }
        return false;
    }
}
