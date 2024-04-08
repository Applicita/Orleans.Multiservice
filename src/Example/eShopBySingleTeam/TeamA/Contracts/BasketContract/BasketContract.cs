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
public record Basket(
    [property: Id(0)] int BuyerId,
    [property: Id(1)] ImmutableArray<BasketItem> Items);

[GenerateSerializer, Immutable]
public record BasketItem(
    [property: Id(0)] int ProductId,
    [property: Id(1)] string ProductName,
    [property: Id(2)] decimal UnitPrice,
    [property: Id(3)] int Quantity);
