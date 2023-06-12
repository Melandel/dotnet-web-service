using Mel.DotnetWebService.Api.ExtensionMethods;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCustomVersionedControllers();
builder.Services.AddCustomSwaggerGenerator();
builder.Services.AddCustomExceptionHandlingCompliantWithRfc7807();

var app = builder.Build();

app.UseCustomExceptionHandlingCompliantWithRfc7807();

app.UseCustomSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCustomControllers();

app.Run();
