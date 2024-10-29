using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace lazycontrol
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit();
#if DEBUG
            builder.Logging.AddDebug();
#endif
#if ANDROID

            builder.Services.AddSingleton<IPlatformService, lazycontrol.Platforms.Android.PlatformService>();
            //builder.Services.AddSingleton<IBluetoothConnector,lazycontrol.Platforms.Android.BluetoothConnector>();
#elif IOS
        builder.Services.AddSingleton<IPlatformService, lazycontrol.Platforms.iOS.PlatformService>();
#elif WINDOWS
            builder.Services.AddSingleton<IPlatformService, lazycontrol.Platforms.Windows.PlatformService>();
            builder.Services.AddSingleton<IBluetoothConnector,lazycontrol.Platforms.Windows.BluetoothConnector>();
#endif
            return builder.Build();
        }
    }
}