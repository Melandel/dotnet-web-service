using Mel.DotnetWebService.Api.Concerns.Routing.AttributeRouteTokenReplacement;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(mvcOptions =>
{
	mvcOptions.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseParameterTransformer()));
});
builder.Services.AddCustomSwaggerGenerator();

var app = builder.Build();
app.UseCustomSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
