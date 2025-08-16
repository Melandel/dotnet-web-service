var builder = WebApplication.CreateBuilder(args);

if (!builder.Configuration.HasAlreadyBeenFullyResolved())
{
	builder.Configuration.ResolveAllSettings(
		builder.Environment,
		"ConfigurationLocationMainIdentifier",
		"ConfigurationLocationFallbackIdentifier");
}

builder.Services.AddCustomSerializationSettings();
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
