using TemplateFirebird.Api.Extensions.Mapping;

namespace TemplateFirebird.Api.Extensions;

public static class MapperExtensions
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        return services.AddAutoMapper(typeof(MappingProfile));
    }
}
