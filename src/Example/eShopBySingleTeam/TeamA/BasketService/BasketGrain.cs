using System.Collections.Immutable;
using System.Globalization;

namespace Applicita.eShop.BasketService;

sealed class BasketGrain : Grain, IBasketGrain
{
    [GenerateSerializer]
    internal sealed class State
    {
        [Id(0)] public bool HasBuyerId { get; set; }
        [Id(1)] public Basket Basket { get; set; } = new Basket(-1, ImmutableArray<BasketItem>.Empty);
    }

    readonly IPersistentState<State> state;
    readonly ICatalogServiceClientGrain catalogServiceClient;

    Basket Basket
    { 
      get
        {
            EnsureStateHasBuyerId();
            return state.State.Basket;

            void EnsureStateHasBuyerId()
            {
                if (state.State.HasBuyerId) return;

                string key = this.GetPrimaryKeyString();
                if (!int.TryParse(key, CultureInfo.InvariantCulture, out int buyerId))
                    throw new ArgumentException($"{nameof(BasketGrain)} key \"{key}\" is not an integer (it must be a buyer id)");

                state.State.Basket = state.State.Basket with { BuyerId = buyerId };
                state.State.HasBuyerId = true;
            }
        }

        set => state.State.Basket = value; 
    }

    public BasketGrain([PersistentState("state")] IPersistentState<State> state)
    {
        this.state = state;
        catalogServiceClient = GrainFactory.GetGrain<ICatalogServiceClientGrain>(ICatalogServiceClientGrain.Key);
    }

    public async Task EmptyBasket() 
    {
        Basket = Basket with { Items = ImmutableArray<BasketItem>.Empty };
        await state.WriteStateAsync();
    }

    public Task<Basket> GetBasket() => Task.FromResult(Basket);

    public async Task<Basket> UpdateBasket(Basket basketToUpdateFrom)
    {
        var updatedBasketItems = await catalogServiceClient.UpdateFromCurrentProducts(basketToUpdateFrom.Items);
        Basket = Basket with { Items = updatedBasketItems };
        await state.WriteStateAsync();
        return Basket;
    }
}
