namespace Applicita.eShop.BasketService;

sealed class BasketGrain : Grain, IBasketGrain
{
    readonly IPersistentState<Basket> basket;

    public BasketGrain([PersistentState("state")] IPersistentState<Basket> basket) => this.basket = basket;

    public async Task EmptyBasket() 
    {
        basket.State = basket.State with { Items = new() };
        await basket.WriteStateAsync();
    }

    public Task<Basket> GetBasket() => Task.FromResult(basket.State);

    public async Task UpdateBasket(Basket basketToUpdateFrom)
    {
        basket.State = basket.State with { Items = basketToUpdateFrom.Items };
        await basket.WriteStateAsync();
    }
}
