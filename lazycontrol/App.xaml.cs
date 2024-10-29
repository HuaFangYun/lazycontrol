

namespace lazycontrol
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Communication communication = new Communication();
            DependencyService.Register<ICommuication, Communication>();
#if ANDROID
            DependencyService.Register<IPlatformService, lazycontrol.Platforms.Android.PlatformService>();
            DependencyService.Register<IBluetoothConnector, lazycontrol.Platforms.Android.BluetoothConnector>();

            MainPage = new  lazycontrol.Platforms.Android.MainPage(DependencyService.Get<IPlatformService>(), DependencyService.Get<IBluetoothConnector>(),communication);
#elif WINDOWS
            DependencyService.Register<IPlatformService, lazycontrol.Platforms.Windows.PlatformService>();
            DependencyService.Register<IBluetoothConnector, lazycontrol.Platforms.Windows.BluetoothConnector>();

            MainPage = new MainPage(DependencyService.Get<IPlatformService>(), DependencyService.Get<IBluetoothConnector>(), communication);
#endif
            //MainPage = new AppShell();
        }
    }
}
