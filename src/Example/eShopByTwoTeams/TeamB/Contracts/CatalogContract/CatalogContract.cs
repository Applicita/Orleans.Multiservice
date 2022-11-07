namespace Applicita.eShop.Contracts.CatalogContract;

[GenerateSerializer, Immutable]
public record Product(
    [property: Id(0)] int Id, 
    [property: Id(1)] string Name, 
    [property: Id(2)] decimal Price);

public interface ICatalogGrain : IGrainWithStringKey
{
    const string Key = "";
    
    /// <returns>The id of the new product</returns>
    Task<int> CreateProduct(Product product);

    Task<ImmutableArray<Product>> GetAllProducts();

    /// <returns>Products for all <paramref name="productIds"/> currently in the catalog; unknown product id's are skipped</returns>
    Task<ImmutableArray<Product>> GetCurrentProducts(ImmutableArray<int> productIds);

    /// <returns>True if <paramref name="product"/> is updated, false if the <paramref name="product"/> <see cref="Product.Id"/> is not found </returns>
    Task<bool> UpdateProduct(Product product);

    /// <returns>True if <paramref name="id"/> is updated, false if the product <paramref name="id"/> is not found </returns>
    Task<bool> DeleteProduct(int id);
}
