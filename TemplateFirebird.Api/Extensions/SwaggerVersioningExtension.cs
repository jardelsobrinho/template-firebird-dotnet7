using Microsoft.AspNetCore.Mvc;

namespace TemplateFirebird.Api.Extensions;

public static class SwaggerVersioningExtension
{
    public static void AddSwaggerVersioningExtension(this IServiceCollection service)
    {
        service.AddApiVersioning(o =>
        {
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new ApiVersion(1, 0);
        });

        service.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });
    }
}
