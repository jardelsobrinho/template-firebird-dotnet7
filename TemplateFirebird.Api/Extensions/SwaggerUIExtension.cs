using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace TemplateFirebird.Api.Extensions;

public static class SwaggerUIExtension
{
    public static void UseSwaggerUiExtension(this IApplicationBuilder app, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        app.UseSwaggerUI(o =>
        {
            o.InjectStylesheet("/swagger-custom.css");
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                o.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    $"Api Sidi - {description.GroupName.ToUpper()}");
            }
        });
    }
}
