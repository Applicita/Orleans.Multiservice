using Applicita.eShop.Contracts.BasketContract;

namespace Applicita.eShop.Apis.BasketApi;

public class BasketsEndpoints(IClusterClient orleans) : IEndpoints
{
    const string Basket = "{buyerId}";

    public void Register(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/baskets").WithTags("Baskets");
        _ = group.MapGet   (Basket, GetBasket);
        _ = group.MapPut   (""    , UpdateBasket);
        _ = group.MapDelete(Basket, EmptyBasket);
    }

    /// <response code="200">The basket of buyerId is returned</response>
    public async Task<Ok<Basket>> GetBasket(int buyerId)
        => Ok(await BasketGrain(buyerId).GetBasket());

    /// <response code="200">The updated basket is returned, with items updated from the current products in the Catalog service</response>
    public async Task<Ok<Basket>> UpdateBasket(Basket basket)
        => Ok(await BasketGrain(basket.BuyerId).UpdateBasket(basket));

    /// <response code="200">The basket of buyerId is emptied</response>
    public async Task<Ok> EmptyBasket(int buyerId)
    { await BasketGrain(buyerId).EmptyBasket(); return Ok(); }

    IBasketGrain BasketGrain(int buyerId)
        => orleans.GetBasketGrain(buyerId);
}
