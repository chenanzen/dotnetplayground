


using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ConsoleApp1;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);


builder.Services.AddScoped<IInputOutputService, ConsoleInputOutputService>();
builder.Services.AddScoped<IInputParserService, InputParserService>();
builder.Services.AddScoped<IVehicleAutomationService, VehicleAutomationService>();
builder.Services.AddTransient<ADCRunner>();

using IHost host = builder.Build();

ExemplifyServiceLifetime(host.Services, "Lifetime 1");

await host.RunAsync();

static void ExemplifyServiceLifetime(IServiceProvider hostProvider, string lifetime)
{
    using IServiceScope serviceScope = hostProvider.CreateScope();

    IServiceProvider provider = serviceScope.ServiceProvider;

    var runner = provider.GetRequiredService<ADCRunner>();
    runner.ADCRunnerExecute();
}