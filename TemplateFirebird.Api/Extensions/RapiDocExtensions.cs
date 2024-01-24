using IGeekFan.AspNetCore.RapiDoc;

namespace TemplateFirebird.Api.Extensions;

public static class RapiDocExtensions
{
    public static void UseRapiDocUICustom(this IApplicationBuilder app)
    {
        app.UseRapiDocUI(c =>
        {
            c.RoutePrefix = ""; // serve the UI at root
            c.SwaggerEndpoint("/v1/api-docs", "V1 Docs");
            //https://mrin9.github.io/RapiDoc/api.html
            //This Config Higher priority
            c.GenericRapiConfig = new GenericRapiConfig()
            {
                RenderStyle = "read",
                Theme = "dark",//light | dark
                SchemaStyle = "tree"////tree | table
            };
        });
    }
}
