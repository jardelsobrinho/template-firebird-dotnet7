namespace TemplateFirebird.Api.Extensions;

public static class CorsExtensions
{
    public static void UseCorsCustom(this IApplicationBuilder app)
    {
        app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
    }
}
