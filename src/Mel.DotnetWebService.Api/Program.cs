using Mel.DotnetWebService.Api.ExtensionMethods;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCustomVersionedControllers();
builder.Services.AddCustomSwaggerGenerator();
builder.Services.AddCustomExceptionHandlingCompliantWithRfc9457();

var app = builder.Build();

app.UseCustomExceptionHandlingCompliantWithRfc9457();

app.UseCustomSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCustomControllers();

app.Run();
