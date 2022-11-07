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
    Contracts.CatalogContract.ICatalogGrain? catalog;

    public async Task<ImmutableArray<BasketItem>> UpdateFromCurrentProducts(ImmutableArray<BasketItem> basketItems)
    {
        catalog ??= GrainFactory.GetGrain<Contracts.CatalogContract.ICatalogGrain>(Contracts.CatalogContract.ICatalogGrain.Key);
        var productIds = basketItems.Select(bi => bi.ProductId).ToImmutableArray();
        var products = await catalog.GetCurrentProducts(productIds);

        List<BasketItem> updatedItems = new();
        foreach (var item in basketItems) 
        {
            var product = products.SingleOrDefault(p => p.Id == item.ProductId);
            if (product is null) continue;

            updatedItems.Add(item with
            {
                ProductName = product.Name,
                UnitPrice = product.Price,
            });
        }
        return updatedItems.ToImmutableArray();
    }
}
