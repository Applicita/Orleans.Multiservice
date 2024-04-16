namespace Applicita.eShop.Apis.Foundation;

public interface IEndpoints
{
    void Register(IEndpointRouteBuilder routeBuilder);
}

public static class WebApplicationExtensions
{
    public static void RegisterEndpoints(this WebApplication app, params Type[] endpointsTypes)
    {
        foreach (var endpointsType in endpointsTypes)
            ((IEndpoints)ActivatorUtilities.CreateInstance(app.Services, endpointsType)).Register(app);
    }
}
