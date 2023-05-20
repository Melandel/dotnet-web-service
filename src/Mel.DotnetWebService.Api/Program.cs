
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCustomControllers();
builder.Services.AddCustomSwaggerGenerator();

var app = builder.Build();
app.UseCustomSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCustomControllers();

app.Run();
