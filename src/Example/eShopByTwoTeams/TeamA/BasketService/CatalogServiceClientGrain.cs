using System.Collections.Immutable;
using Orleans.Concurrency;

namespace Applicita.eShop.BasketService;

interface ICatalogServiceClientGrain : IGrainWithIntegerKey
{
    const int Key = 0;
    Task<ImmutableArray<BasketItem>> UpdateFromCurrentProducts(ImmutableArray<BasketItem> basketItems);
}

[StatelessWorker]
sealed class CatalogServiceClientGrain : Grain, ICatalogServiceClientGrain
{
    readonly CatalogServiceClient client = new("http://localhost:5113", new());

    public async Task<ImmutableArray<BasketItem>> UpdateFromCurrentProducts(ImmutableArray<BasketItem> basketItems)
    {
        var productIds = basketItems.Select(bi => bi.ProductId).ToImmutableArray();
        var products = await client.ProductsAllAsync(productIds);

        List<BasketItem> updatedItems = [];
        foreach (var item in basketItems)
        {
            var product = products.SingleOrDefault(p => p.Id == item.ProductId);
            if (product is null) continue;

            updatedItems.Add(item with
            {
                ProductName = product.Name,
                UnitPrice = (decimal)product.Price,
            });
        }
        return [.. updatedItems];
    }
}
