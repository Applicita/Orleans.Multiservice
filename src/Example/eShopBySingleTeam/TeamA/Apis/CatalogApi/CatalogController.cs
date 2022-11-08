using System.Collections.Immutable;
using Applicita.eShop.Contracts.CatalogContract;

namespace Applicita.eShop.Apis.CatalogApi;

[Route("[controller]")]
[ApiController]
public class CatalogController : ControllerBase
{
    const string Products = "products";
    const string Product = Products + "/{id}";

    readonly ICatalogGrain catalog;

    public CatalogController(IClusterClient orleans)
        => catalog = orleans.GetGrain<ICatalogGrain>(ICatalogGrain.Key);

    /// <response code="201">The new product is created with the returned id</response>
    [HttpPost(Products)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<int>> CreateProduct(Product product)
        => CreatedAtAction(nameof(CreateProduct), await catalog.CreateProduct(product));

    /// <response code="200">
    /// Products for all <paramref name="id"/>'s currently in the catalog are returned; unknown product id's are skipped.
    /// If no <paramref name="id"/>'s are specified, all products in the catalog are returned
    /// </response>
    [HttpGet(Products)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ImmutableArray<Product>> GetProducts([FromQuery] int[]? id = null)
        => await (id is null ? catalog.GetAllProducts() : catalog.GetCurrentProducts(id.ToImmutableArray()));

    /// <response code="200">The product is updated</response>
    /// <response code="404">The product id is not found</response>
    [HttpPut(Products)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateProduct(Product product)
        => (await catalog.UpdateProduct(product)) ? Ok() : NotFound(product.Id);

    /// <response code="200">The product is deleted</response>
    /// <response code="404">The product id is not found</response>
    [HttpDelete(Product)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteProduct(int id)
        => (await catalog.DeleteProduct(id)) ? Ok() : NotFound(id);
}
