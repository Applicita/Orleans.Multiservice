using System.Collections.Immutable;
using System.Globalization;

namespace Applicita.eShop.Contracts.BasketContract;

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

public interface IBasketGrain : IGrainWithStringKey
{
    static string Key(int buyerId) => buyerId.ToString(CultureInfo.InvariantCulture);

    Task<Basket> GetBasket();

    Task<Basket> UpdateBasket(Basket basket);

    Task EmptyBasket();
}
