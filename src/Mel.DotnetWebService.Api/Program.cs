
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCustomControllers();
builder.Services.AddCustomSwaggerGenerator();
builder.Services.AddCustomExceptionHandlingCompliantWithRfc9457();

var app = builder.Build();

app.UseCustomExceptionHandlingCompliantWithRfc9457();

app.UseCustomSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCustomControllers();

app.Run();
