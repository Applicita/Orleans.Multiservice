namespace Applicita.eShop.Apis.Catalog;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    [HttpGet]
    public string GetHello() => "Hello from Catalog";
}
