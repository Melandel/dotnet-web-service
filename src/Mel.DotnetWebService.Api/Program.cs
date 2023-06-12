using Mel.DotnetWebService.Api.ExtensionMethods;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCustomVersionedControllers();
builder.Services.AddCustomSwaggerGenerator();

var app = builder.Build();
app.UseCustomSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
