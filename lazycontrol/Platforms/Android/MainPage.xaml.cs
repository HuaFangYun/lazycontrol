using Android.Bluetooth.LE;
using Android.Bluetooth;
using System.Diagnostics;
using AndroidX.AppCompat.App;
using Android.Content;
using Kotlin.Contracts;
using System.Collections;
using Android.Views;
using Plugin.BLE;
using Android.OS;
using Java.Util;
using ScanMode = Android.Bluetooth.LE.ScanMode;
using Android.Runtime;
using Android.Media.Metrics;
using Java.IO;
using Console = System.Console;

namespace lazycontrol.Platforms.Android;

public partial class MainPage : ContentPage
{
    public  IBluetoothConnector _bluetoothConnector;
    private readonly IPlatformService _platformService;
    public ICommuication _communication;
    
    public MainPage(IPlatformService platformService,IBluetoothConnector connector,ICommuication commuication)
    {
        InitializeComponent();
        _platformService = platformService;
        _bluetoothConnector = connector;
        _communication = commuication;
        DisplayPlatformMessage();
        this.Loaded += MainPage_Loaded;
#if ANDROID
        var btnscan = FindByName("BTNScan") as Button;
        btnscan.Clicked += OnButtonClicked;
#endif
    }

    private void MainPage_Loaded(object? sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine(_bluetoothConnector);
        _bluetoothConnector.bs += _bluetoothConnector_bs;
    }

    private void _bluetoothConnector_bs(object obj)
    {
        var s = (OutputStream)obj;
        s.WriteAsync(Convert.FromBase64String("sfkjh"));
        
    }

    private void Btnscan_Clicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public void BlueAdvert()
    {
        BluetoothLeScanner scanner = BluetoothAdapter.DefaultAdapter?.BluetoothLeScanner;
        ScanFilter filter = new ScanFilter.Builder()
            ?.SetServiceUuid(new ParcelUuid(UUID.FromString("12345678-1234-5678-1234-567812345678")))
            ?.SetManufacturerData(0xAA34, new byte[] { 1 })  // Filter by app version
            ?.Build();
        ScanSettings settings = new ScanSettings.Builder()
            ?.SetScanMode(ScanMode.LowLatency)
            ?.Build();

        List<ScanFilter> filters = new List<ScanFilter> { filter };
        List<ScanResult> ScanReslt = new List<ScanResult>();
        var scan = new Scan();
        scanner?.StartScan(filters, settings, scan);
        scan?.OnBatchScanResults(ScanReslt);
        
       
    }


   
    private void OnButtonClicked(object sender, EventArgs e)
    {
        //DisplayAlert("Alert", "Android button clicked!", "OK");
        //BluConnect();
        //BlueAdvert();
        BluConnect();
    }

    private void DisplayPlatformMessage()
    {
        DisplayAlert("Platform Message", _platformService?.GetPlatformMessage(), "OK");
    }
    public async Task<bool> CheckBluetoothAccess()
    {
        try
        {
            var requestStatus = await Permissions.CheckStatusAsync<BluetoothPermissions>();
            return requestStatus == PermissionStatus.Granted;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Oops  {ex}");
            return false;
        }
    }

    public async Task<bool> RequestBluetoothAccess()
    {
        try
        {
            var requestStatus = await Permissions.RequestAsync<BluetoothPermissions>();
            return requestStatus == PermissionStatus.Granted;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Oops  {ex}");
            return false;
        }
    }
    private ICollection<BluetoothDevice> PairedDevices()
    {
        var bluetoothManager = MainApplication.Current?.BaseContext?.GetSystemService(Context.BluetoothService) as BluetoothManager;
        var bluetoothAdapter = bluetoothManager?.Adapter;
        var bluetoothfind = bluetoothAdapter;
        //BluetoothLeScanner bluetoothfind;
       
        var pairedDevices = bluetoothAdapter?.BondedDevices;
        
        foreach (var device in pairedDevices)
        {
            Console.WriteLine($"Device Name: {device.Name}, Address: {device.Address}");
        }
        return pairedDevices;
    }
    private async void BluConnect()
    {
        var ble = CrossBluetoothLE.Current;
        var adapter = CrossBluetoothLE.Current.Adapter;
        var state = ble.State;
        var devices = new List<Plugin.BLE.Abstractions.Contracts.IDevice>();

        //PairedDevices();
        var grid = FindByName("BtDevice") as Grid;
        var bluetoothService = new BluetoothService(this);
        BluetoothService.setMainPage(this);
        bluetoothService?.StartDiscovery();

        if (!await CheckBluetoothAccess())
        {
            await RequestBluetoothAccess();
        }

        //foreach (var device in BluetoothService.Devices)
        //{
        //    //System.Diagnostics.Debug.WriteLine($"device: {device.Name} - {device.Address}");

        //}

        //if (state == Plugin.BLE.Abstractions.Contracts.BluetoothState.On)
        //{

        //    adapter.DeviceDiscovered += (s, a) =>
        //    {
        //        devices.Add(a.Device);
        //       //Debug.WriteLine($"Discovered device: {a.Device.Id}:{a.Device.NativeDevice}:{a.Device.Name}:{a.Device.GetType()}:{(s as BluetoothAdapter)?.Name}");
        //    };
        //    //var systemDevices = adapter.GetSystemConnectedOrPairedDevices();

        //    //foreach (var device in systemDevices)
        //    //{
        //    //    await adapter.ConnectToDeviceAsync(device);
        //    //}
        //    await adapter.StartScanningForDevicesAsync();
        //    await Task.Delay(100);
        //    await adapter.StopScanningForDevicesAsync();

        //    foreach(var device in devices)
        //    {
        //       //Debug.WriteLine($"Discovered device:{device.Name}: {device.Id}:{device.NativeDevice}:{device.Name}:{(device.NativeDevice as BluetoothDevice)?.Name}");
        //    }
        //}
        //else
        //{
        //    System.Diagnostics.Debug.WriteLine("Bluetooth is off.");
        //}
    }
    class Scan : ScanCallback
    {
        public Scan() : base()
        {
            
        
        }

        public override void OnScanFailed([GeneratedEnum] ScanFailure errorCode)
        {
            base.OnScanFailed(errorCode);
        }
        public override void OnScanResult([GeneratedEnum] ScanCallbackType callbackType, ScanResult? result)
        {
            base.OnScanResult(callbackType, result);
            System.Diagnostics.Debug.WriteLine(result.AdvertisingSid.ToString());
        }

        internal void OnScanResult(ScanCallbackType allMatches, Action value)
        {
            System.Diagnostics.Debug.WriteLine(value); 
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {

    }
}