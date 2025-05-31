using CommunityToolkit.Maui;
using maui_heater_manager.Pages;
using maui_heater_manager.ViewModels;
using Microsoft.Extensions.Logging;
using Serilog;

namespace maui_heater_manager;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseMauiCommunityToolkit();

        BuildLogging();
        BuildBinding(builder.Services);
        BuildService(builder.Services);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static void BuildLogging()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs/heater_log_", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    private static void BuildBinding(IServiceCollection services)
    {
        // Add page + viewModel
        services.AddTransient<NowPage, NowViewModel>();
        services.AddTransient<UsagePage, UsageViewModel>();
        services.AddTransient<DevicePage, DeviceViewModel>();
        services.AddTransient<SettingPage, SettingViewModel>();

        //Routing.RegisterRoute(nameof(APage), typeof(APage));
    }

    private static void BuildService(IServiceCollection services)
    {

    }
}
