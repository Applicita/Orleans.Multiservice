﻿using Applicita.eShop.Contracts.BasketContract;

namespace Applicita.eShop.Apis.BasketApi;

[Route("[controller]")]
[ApiController]
public class BasketsController : ControllerBase
{
    const string Basket = "{buyerId}";

    readonly IClusterClient orleans;

    public BasketsController(IClusterClient orleans)
        => this.orleans = orleans;

    /// <response code="200">The basket of buyerId is returned</response>
    [HttpGet(Basket)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Basket>> GetBasket(int buyerId)
        => Ok(await BasketGrain(buyerId).GetBasket());

    /// <response code="200">The updated basket is returned, with items updated from the current products in the Catalog service</response>
    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Basket>> UpdateBasket(Basket basket)
        => Ok(await BasketGrain(basket.BuyerId).UpdateBasket(basket));

    /// <response code="200">The basket of buyerId is emptied</response>
    [HttpDelete(Basket)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> EmptyBasket(int buyerId)
    { await BasketGrain(buyerId).EmptyBasket(); return Ok(); }

    IBasketGrain BasketGrain(int buyerId)
        => orleans.GetBasketGrain(buyerId);
}
