using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE;
using System.Diagnostics;
using Microsoft.Maui.Controls.PlatformConfiguration;
using lazycontrol.Platforms;
using System.Text;
#if WINDOWS
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using lazycontrol.Platforms.Windows;
#endif





namespace lazycontrol
{
    public partial class MainPage : ContentPage
    {

        private readonly IPlatformService _platformService;
        private bool _conntected = false;
        private IBluetoothConnector connector;
        private ICommuication commuication;
        public MainPage(IPlatformService platformService,IBluetoothConnector connect,ICommuication commuicate)
        {
            InitializeComponent();
            _platformService = platformService;
            connector = connect;
            commuication = commuicate;
            this.Loaded += MainPage_Loaded;
            


        }

        private void MainPage_Loaded(object? sender, EventArgs e)
        {

            //
#if WINDOWS
            //DisplayPlatformMessage();
            BlueAddvert();
            try
            {
                //connector = DependencyService.Get<IBluetoothConnector>();
                //var s = DependencyService.Resolve<IBluetoothConnector>();
                //connector = new BluetoothConnector();
                //_bluetoothConnector = new lazycontrol.Platforms.Windows.BluetoothConnector();
                
                connector?.DisplayConnectedBluetoothDevices();

                connector.IsConnect += Connector_IsConnected;
                connector.DataReceived += data =>
                {

                    DisplayAlert("Data received:", BitConverter.ToString(data), "Cancel");
                    Console.WriteLine($"Data received: {BitConverter.ToString(data)}");

                };
                commuication.DataReceived += data =>
                {
                    Debug.WriteLine(Encoding.Default.GetString(data));
                };
                connector.bs += Connector_bs;


            }
            catch (Exception ex) { 
            }
#endif
            //
            //BluConnect();
            
           
           
        }

        private async void Connector_bs(Object obj)
        {
#if WINDOWS
            var o = (IInputStream)obj;
            await o.ReadAsync(new Windows.Storage.Streams.Buffer(1024), (uint)1024, InputStreamOptions.Partial);
#endif

        }

        private void Connector_IsConnected(Object obj)
        {
            Debug.WriteLine("Here");
            _conntected = true;
            
            DisplayAlert("Connected", _conntected.ToString(), "OK");
#if WINDOWS
            
            
           
#endif

        }

        //private void OnButtonClicked(object sender, EventArgs e)
        //{
        //    DisplayAlert("Alert", "Windows button clicked!", "OK");
        //}

        private void DisplayPlatformMessage()
        {
           
            DisplayAlert("Platform Message", _platformService?.GetPlatformMessage(), "OK");
        }
        private void BlueAddvert()
        {
            BluetoothLEAddvertisingService service = new BluetoothLEAddvertisingService();
#if WINDOWS
            service.StartAdvertising();
#endif
        }
        private async void BluConnect()
        {
            var ble = CrossBluetoothLE.Current;
            var adapter = CrossBluetoothLE.Current.Adapter;
            var state = ble.State;

            if (state == BluetoothState.On)
            {
                adapter.DeviceDiscovered += (s, a) =>
                {
                    Debug.WriteLine($"Discovered device: {a.Device.Id}:{a.Device.NativeDevice}:{a.Device.Name}");
                    var label = FindByName("Las") as Label;
                    //label.Text += a.Device.Name.ToString();
                };

                await adapter.StartScanningForDevicesAsync();
            }
            else
            {
                Debug.WriteLine("Bluetooth is off.");
            }
        }
    }

}
