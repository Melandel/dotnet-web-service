var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCustomControllersAndCustomApiVersioning();
builder.Services.AddCustomSwaggerGeneration();
builder.Services.AddCustomSwaggerUI();
builder.Services.AddCustomExceptionHandlingCompliantWithRfc9457();
builder.Services.AddCustomRuntimeValidation();

var app = builder.Build();

app.UseCustomExceptionHandlingCompliantWithRfc9457();

app.UseSwagger();

app.UseSwaggerUI();

app.UseCustomRewriter();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCustomControllers();

app.ExecuteRuntimeValidations();

app.Run();
