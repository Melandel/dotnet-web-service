var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddCustomSwaggerGeneration();
builder.Services.AddCustomSwaggerUI();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
