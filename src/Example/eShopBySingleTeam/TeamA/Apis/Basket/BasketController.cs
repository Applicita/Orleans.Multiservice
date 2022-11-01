namespace Apis.Basket;

[ApiController]
[Route("[controller]")]
public class BasketController : ControllerBase
{
    [HttpGet]
    public string GetHello() => "Hello from Basket";
}
