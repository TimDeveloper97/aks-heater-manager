using CommunityToolkit.Maui;
using LiveChartsCore.SkiaSharpView.Maui;
using maui_heater_manager.Pages;
using maui_heater_manager.Pages.Mains;
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
                fonts.AddFont("fa-solid-900.otf", "FontAwesomeSolid");
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
        services.AddTransient<NowsView, NowViewModel>();
        services.AddTransient<HistoryView, UsageViewModel>();
        services.AddTransient<DevicesView, DeviceViewModel>();
        services.AddTransient<SettingView, SettingViewModel>();
        services.AddTransient<AccountPage, AccountViewModel>();
        services.AddTransient<LoginPage, LoginViewModel>();
        services.AddTransient<MainPage, MainViewModel>();
        services.AddTransient<StatePage, StateViewModel>();

        // Add router
        Routing.RegisterRoute(nameof(AccountPage), typeof(AccountPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(StatePage), typeof(StatePage));

        Routing.RegisterRoute(nameof(NowsView), typeof(NowsView));
        Routing.RegisterRoute(nameof(HistoryView), typeof(HistoryView));
        Routing.RegisterRoute(nameof(SettingView), typeof(SettingView));
        Routing.RegisterRoute(nameof(DevicesView), typeof(DevicesView));
    }

    private static void BuildService(IServiceCollection services)
    {

    }
}
