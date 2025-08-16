using Mel.DotnetWebService.Api.Concerns.Configurability;
using Mel.DotnetWebService.CrossCuttingConcerns.Configurability;
using Mel.DotnetWebService.CrossCuttingConcerns.Configurability.ConfigurationKeyFiltering;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Configuration.HasAlreadyBeenFullyResolved())
{
	builder.Configuration.ResolveAllSettings(
		builder.Environment,
		Integration.SingleConfigurationSettingRedirectingToTheFullConfiguration.ConfigurationLocationMainIdentifier,
		Integration.SingleConfigurationSettingRedirectingToTheFullConfiguration.ConfigurationLocationFallbackIdentifier,
		ConfigurationKeyFilters.ExplicitPublicAndImplicitPrivate);
}

builder.Services.AddCustomSerializationSettings();
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
