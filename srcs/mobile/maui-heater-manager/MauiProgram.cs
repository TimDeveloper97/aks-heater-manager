using CommunityToolkit.Maui;
using LiveChartsCore.SkiaSharpView.Maui;
using maui_heater_manager.Pages;
using maui_heater_manager.Pages.Settings;
using maui_heater_manager.ViewModels;
using maui_heater_manager.ViewModels.Settings;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Serilog;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace maui_heater_manager;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        // github chart
        // https://www.nuget.org/packages/LiveChartsCore.SkiaSharpView.Maui
        // link: https://livecharts.dev/docs/maui/2.0.0-rc5.4/gallery

        // CommunityToolkit.Maui
        // CommunityToolkit.Mvvm

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseSkiaSharp()
            .UseLiveCharts()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseMauiCommunityToolkit()
            .UseLocalNotification();

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
        services.AddTransient<AccountPage, AccountViewModel>();

        // Add router
        Routing.RegisterRoute(nameof(AccountPage), typeof(AccountPage));
    }

    private static void BuildService(IServiceCollection services)
    {

    }
}
