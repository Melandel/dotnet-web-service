var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCustomControllers();
builder.Services.AddCustomSwaggerGeneration();
builder.Services.AddCustomSwaggerUI();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseCustomRewriter();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
