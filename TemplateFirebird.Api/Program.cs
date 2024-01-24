using MediatR;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Globalization;
using System.Text;
using TemplateFirebird.Api.Authorization;
using TemplateFirebird.Api.Extensions;
using TemplateFirebird.Application.Shared;
using TemplateFirebird.Shared;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); //Registra o charset win1252

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AppSettings).Assembly));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddMapper();
builder.Services.AddApplication();
builder.Services.AddInfra(args);

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
});

builder.Services.AddSwaggerVersioningExtension();
builder.Services.AddProducesResponse();
builder.Services.AddProblemDetailsModelState();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerCustom();

var app = builder.Build();
app.UseErrorHandler();

app.UseSwagger();
app.UseSwaggerUiExtension(app.Services.GetRequiredService<IApiVersionDescriptionProvider>());
app.UseRapiDocUICustom();

//App supported cultures
var supportedCultures = new[]
{
        new CultureInfo("pt-BR"),
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("pt-BR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new List<IRequestCultureProvider> { new QueryStringRequestCultureProvider() }
});

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCorsCustom();
app.UseMiddleware<JwtMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.MapSwagger("{documentName}/api-docs");

app.Run();