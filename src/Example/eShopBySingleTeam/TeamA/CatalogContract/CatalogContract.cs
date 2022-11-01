namespace Applicita.eShop.CatalogContract;

[GenerateSerializer, Immutable]
public record Product(
    [property: Id(0)] int Id, 
    [property: Id(1)] string Name, 
    [property: Id(2)] decimal Price);

public interface ICatalogGrain : IGrainWithStringKey
{
    const string Key = "";
    
    Task<int> CreateProduct(Product product);

    Task<ImmutableArray<Product>> GetAllProducts();

    Task UpdateProduct(Product product);

    Task DeleteProduct(int id);
}
