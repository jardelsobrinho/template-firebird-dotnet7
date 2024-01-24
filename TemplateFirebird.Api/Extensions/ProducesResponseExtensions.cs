using Microsoft.AspNetCore.Mvc;

namespace TemplateFirebird.Api.Extensions;

public static class ProducesResponseExtensions
{
    public static void AddProducesResponse(this IServiceCollection services)
    {
        services.AddMvc(options =>
        {
            options.Filters.Add(new ProducesAttribute("application/json"));
            options.Filters.Add(new ConsumesAttribute("application/json"));
            options.Filters.Add(new ProducesResponseTypeAttribute(400));
            options.Filters.Add(new ProducesResponseTypeAttribute(404));
            options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), 500));
        });
    }
}
