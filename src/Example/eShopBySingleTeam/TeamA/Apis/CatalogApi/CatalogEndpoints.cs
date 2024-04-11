using Applicita.eShop.Contracts.CatalogContract;

namespace Applicita.eShop.Apis.CatalogApi;

public class CatalogEndpoints(IClusterClient orleans) : IEndpoints
{
    const string Products = "/products";
    const string Product  = Products + "/{id}";

    readonly ICatalogGrain catalog = orleans.GetGrain<ICatalogGrain>(ICatalogGrain.Key);

    public void Register(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/mcatalog").WithTags("Catalog");
        _ = group.MapPost  (Products, CreateProduct);
        _ = group.MapGet   (Products, GetProducts  ).WithName(nameof(GetProducts));
        _ = group.MapPut   (Products, UpdateProduct);
        _ = group.MapDelete(Product , DeleteProduct);
    }

    /// <response code="201">The new product is created with the returned id</response>
    async Task<CreatedAtRoute<int>> CreateProduct(Product product)
    {
        int id = await catalog.CreateProduct(product);
        return CreatedAtRoute(id, nameof(GetProducts), new { id });
    }

    /// <response code="200">
    /// Products for all <paramref name="id"/>'s currently in the catalog are returned;
    /// unknown product id's are skipped.
    /// If no <paramref name="id"/>'s are specified, all products in the catalog are returned
    /// </response>
    async Task<Ok<ImmutableArray<Product>>> GetProducts(int[]? id) => Ok(
        id?.Length > 0
        ? await catalog.GetCurrentProducts([.. id])
        : await catalog.GetAllProducts()
    );

    /// <response code="200">The product is updated</response>
    /// <response code="404">The product id is not found</response>
    public async Task<Results<Ok, NotFound<int>>> UpdateProduct(Product product)
        => await catalog.UpdateProduct(product)
        ? Ok()
        : NotFound(product.Id);

    /// <response code="200">The product is deleted</response>
    /// <response code="404">The product id is not found</response>
    public async Task<Results<Ok, NotFound<int>>> DeleteProduct(int id)
        => await catalog.DeleteProduct(id)
        ? Ok()
        : NotFound(id);
}
