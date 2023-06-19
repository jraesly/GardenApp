using Microsoft.Maui.LifecycleEvents;
using WeatherTwentyOne.Helpers;
using WeatherTwentyOne.Interfaces;
using WeatherTwentyOne.Pages;
using WeatherTwentyOne.ViewModels;

namespace WeatherTwentyOne;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => {
                fonts.AddFont("fa-solid-900.ttf", "FontAwesome");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
            });
        builder.ConfigureLifecycleEvents(lifecycle => {
#if WINDOWS
        //lifecycle
        //    .AddWindows(windows =>
        //        windows.OnNativeMessage((app, args) => {
        //            if (WindowExtensions.Hwnd == IntPtr.Zero)
        //            {
        //                WindowExtensions.Hwnd = args.Hwnd;
        //                WindowExtensions.SetIcon("Platforms/Windows/trayicon.ico");
        //            }
        //        }));

            lifecycle.AddWindows(windows => windows.OnWindowCreated((del) => {
                del.ExtendsContentIntoTitleBar = true;
            }));
#endif
        })
        .ConfigureEssentials(essentials =>
        {
            essentials.UseMapServiceToken(Constants. BingMapsAPIKey);
        }); ;

        var services = builder.Services;
#if WINDOWS
            services.AddSingleton<ITrayService, WinUI.TrayService>();
            services.AddSingleton<INotificationService, WinUI.NotificationService>();
#elif MACCATALYST
            services.AddSingleton<ITrayService, MacCatalyst.TrayService>();
            services.AddSingleton<INotificationService, MacCatalyst.NotificationService>();
#endif
        services.AddSingleton<HomeViewModel>();
        services.AddSingleton<HomePage>();

        // Add your services here
        services.AddSingleton<IWeatherUpdateService, WeatherUpdateService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IFavoriteLocationsService, FavoriteLocationsService>();
        services.AddTransient<FavoritesPage>();
        services.AddTransient<FavoritesViewModel>();
        services.AddSingleton<ILocationTitleService, LocationTitleService>();
        services.AddSingleton<RestService>();


        return builder.Build();
    }
}