using System.Globalization;

namespace Applicita.eShop.Contracts.BasketContract;

public static class GrainFactoryExtensions
{
    public static IBasketGrain GetBasketGrain(this IGrainFactory factory, int buyerId)
    {
        ArgumentNullException.ThrowIfNull(factory);
        return factory.GetGrain<IBasketGrain>(buyerId.ToString(CultureInfo.InvariantCulture));
    }
}

public interface IBasketGrain : IGrainWithStringKey
{
    Task<Basket> GetBasket();

    Task<Basket> UpdateBasket(Basket basket);

    Task EmptyBasket();
}

[GenerateSerializer, Immutable]
public record Basket(int BuyerId, ImmutableArray<BasketItem> Items);

[GenerateSerializer, Immutable]
public record BasketItem(int ProductId, string ProductName, decimal UnitPrice, int Quantity);
