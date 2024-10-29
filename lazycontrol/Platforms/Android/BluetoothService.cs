using Android.Bluetooth;
using Android.Content;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.BroadcastReceivers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lazycontrol.Platforms.Android
{
    public class BluetoothService : IBluetoothService
    {
        private static List<BluetoothDevice> _devices;
        private static List<BluetoothDevice> _connectedDevices;
        BluetoothAdapter mBluetoothAdapter;
        private static MainPage mMainPage;
        public static List<BluetoothDevice> Devices { get { return _devices; } set { value = _devices; } }

        
        
        public BluetoothService(MainPage page) 
        {
            mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            mMainPage = page;
        }

        public static void setMainPage(MainPage main)
        {
            mMainPage = main;
            Debug.WriteLine(mMainPage.ToString());
        }
        public static MainPage getMainPage()
        {

            return mMainPage;
        }
        public void StartDiscovery()
        {
            if(mBluetoothAdapter == null || !mBluetoothAdapter.IsEnabled) 
            {
                Console.WriteLine("Bluetooth is not enabled.");
                return;
            }

            var pairedDevices = mBluetoothAdapter.BondedDevices;

            foreach (BluetoothDevice device in pairedDevices)
            {
                Console.WriteLine($"Paired devices: {device.Name} - {device.Address}");
               
                //Devices?.Add(device);
                build(device);

            }

            var receiver = new BluetoothReceiver();
            MainApplication.Context.RegisterReceiver(receiver, new IntentFilter(BluetoothDevice.ActionFound));
            
            mBluetoothAdapter.StartDiscovery();
            
        }

        private void MMainPage_HandlerChanged(object? sender, EventArgs e)
        {
            Debug.WriteLine("aslkjdhkjafhs");
        }

        private void B_HandlerChanged(object? sender, EventArgs e)
        {
            

        }

        public static void build(BluetoothDevice device)
        {
            var b = mMainPage.FindByName("BtDevice") as FlexLayout;
            
            Debug.WriteLine(b);
            Label t = new Label();
            ImageButton image = new ImageButton();
            t.Text = device?.Name;
            t.TextColor = Colors.Black;
            t.LineBreakMode = LineBreakMode.WordWrap;
            t.WidthRequest = 100;
            t.HorizontalOptions = LayoutOptions.Center;
            //LineBreakMode = "WordWrap" 
            t.Padding = new Thickness(20, 20);
           
            
            AbsoluteLayout layout = new AbsoluteLayout();
            layout.Margin = new Thickness(2, 2);
            image.BackgroundColor = Colors.Aqua;
                       
            image.WidthRequest = 100;
            image.HeightRequest = 100;
            
            
            layout?.Add(image);
            layout?.Add(t);

            b?.Add(layout);
            image.Clicked += (s, e) =>
            {
                Debug.WriteLine($"{device?.Name} - {device?.Address}");
                var rpage = new SecondPage(mMainPage._bluetoothConnector,mMainPage._communication);
                rpage.CurrentDevice = device;
                App.Current.MainPage = new NavigationPage(rpage);
            };
          
        }
        
        private class BluetoothReceiver : BroadcastReceiver
        {
            protected override void JavaFinalize()
            {
                base.JavaFinalize();
                Debug.WriteLine("jsdhfkjhasdf");
            }
            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                Debug.WriteLine("jsdhfkjhasdfddedee");
            }
            public override global::Android.OS.IBinder? PeekService(Context? myContext, Intent? service)
            {
                Debug.WriteLine("jsdhfkjhasdfddedee;fdsjgkldsjfg");
                return base.PeekService(myContext, service);
            }

            public override void OnReceive(Context context, Intent intent)
            {
                try
                {
                    if (BluetoothDevice.ActionFound.Equals(intent.Action))
                    {
                        BluetoothDevice device = (BluetoothDevice)intent?.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                        if (device != null)
                        {
                            if (device?.Name != "" && device?.Name?.Length > 0)
                            {
                                //BluetoothService.Devices?.Add(device);
                                BluetoothService.build(device);
                            }

                        }
                        
                        Console.WriteLine($"Discovered device: {device?.Name} - {device?.DescribeContents()}");
                    }
                }
                catch (Exception ex) {

                    Debug.WriteLine(ex);

                } finally {
                   


                }
                

              
            }
            
        }
    }

}
