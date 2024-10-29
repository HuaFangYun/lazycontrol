using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;

using Windows.Storage.Streams;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using InTheHand.Net;
using lazycontrol.Platforms.Windows;
using Microsoft.UI.Xaml.Media;
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
using System.Runtime.InteropServices.WindowsRuntime;
using Buffer = Windows.Storage.Streams.Buffer;
using Windows.Networking.Sockets;
using System.Net.Sockets;
using System.Net;
using System.Security.Authentication.ExtendedProtection;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;

[assembly:Dependency(typeof(BluetoothConnector))]
namespace lazycontrol.Platforms.Windows
{

    public class BluetoothConnector : IBluetoothConnector
    {
        private StreamSocket _socket;
        private DataReader _reader;
      
    

        // Define the event that is triggered when data is received
        public event Action<byte[]> DataReceived;
        public event Action<Object> IsConnect;
        public event Action<Object> bs;

        public async Task<bool> ConnectToDevice(Object deviceId)
        {
            try
            {
                var bluetoothDevice = await BluetoothDevice.FromIdAsync(deviceId as string);
                BluetoothAdapter bluetoothAdapter = await BluetoothAdapter.GetDefaultAsync();
              
                if (bluetoothDevice != null)
                {
                    var rfcommServices = await bluetoothDevice.GetRfcommServicesAsync();
                    var service = rfcommServices.Services[0];
                    bluetoothDevice.ConnectionStatusChanged += BluetoothConnector_ConnectionStatusChanged;

                   
                    //var S = await bluetoothDevice.GetRfcommServicesForIdAsync(service.ServiceId);
                    
                    _socket = new StreamSocket();
                    
                    await _socket.ConnectAsync(service.ConnectionHostName, service.ConnectionServiceName);
                    


                    //await listener.EnableTransferOwnership(Guid.Parse(service.ServiceId.Uuid.ToString()));



                    //await listener?.BindEndpointAsync(service.ConnectionHostName, service.ConnectionServiceName);

                    //listener.ConnectionReceived += Listener_ConnectionReceived;

                    //rfcommServices.Services[0].

                    //await listener.
                    //listener.ConnectionReceived += Listener_ConnectionReceived;

                    IsConnect.Invoke(_socket);
                    StartListeningForData(); // Start listening for data
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error connecting to device: {ex.Message}");
            }

            return false;
        }

        private void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Debug.WriteLine("zjsdkjasdhgfjkghdfgjksdj");
            Debug.WriteLine(args.Socket.InputStream);
           
        }

     

        private async void StartListeningForData()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            bs.Invoke(_socket.InputStream);
            //var d = await ReceiveData();
            





            //Task.Run(async () =>
            //{
            //    while (true)
            //    {
            //        try
            //        {

            //            //_reader = new DataReader(_socket.InputStream);
            //            //var bytesRead = _reader?.LoadAsync(1024); // You can adjust buffer size
            //            //bytesRead.AsTask().Wait();
            //            //Debug.WriteLine(bytesRead?.Completed);
            //        }
            //        catch (Exception ex) { 

            //        }

            //    }
            //    //while (true)
            //    //{
            //    //    try
            //    //    {
            //    //        // Load data from the input stream
            //    //        //var  bytesRead = _reader?.LoadAsync(1024); // You can adjust buffer size

            //    //        //Debug.WriteLine(bytesRead?.Completed);

            //    //        //if (bytesRead > 0)
            //    //        //{
            //    //        //    byte[] buffer = new byte[bytesRead];
            //    //        //    _reader.ReadBytes(buffer);

            //    //        //    // Trigger the event when data is received
            //    //        //    DataReceived?.Invoke(buffer);
            //    //        //}
            //    //    }
            //    //    catch (Exception ex)
            //    //    {
            //    //        Debug.WriteLine($"Error receiving data: {ex.Message}");
            //    //    }
            //    //}
            //});
        }

        public async Task DisconnectDevice()
        {
            _socket?.Dispose();
        }
        public async Task DisplayConnectedBluetoothDevices()
        {
            var connectedDevices = await GetConnectedBluetoothDevices();
            foreach (var device in connectedDevices)
            {

                Debug.WriteLine($"Connected Device ID: {device.DeviceId}, Name: {device.Name}");
            }
        }
        public async Task<List<BluetoothDevice>> GetConnectedBluetoothDevices()
        {
            List<BluetoothDevice> connectedDevices = new List<BluetoothDevice>();

            // Query for all paired Bluetooth devices
            var devices = await DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelectorFromPairingState(true), new string[] { "System.Devices.Aep.IsConnected" });
          
            foreach (var deviceInfo in devices)
            {
                // Get Bluetooth device object from ID
                BluetoothDevice bluetoothDevice = await BluetoothDevice.FromIdAsync(deviceInfo.Id);
                
                if (bluetoothDevice != null && bluetoothDevice.ConnectionStatus == BluetoothConnectionStatus.Connected)
                {
                    // Add connected device to list
                    connectedDevices.Add(bluetoothDevice);
                }
            }
            connectedDevices[0].ConnectionStatusChanged += BluetoothConnector_ConnectionStatusChanged;
            await ConnectToDevice(connectedDevices[0].DeviceId);
            
            connectedDevices[0].SdpRecordsChanged += BluetoothConnector_SdpRecordsChanged;

            
           
            
            DataReceived += BluetoothConnector_DataReceived;
            return connectedDevices;
        }

       

        private void BluetoothConnector_SdpRecordsChanged(BluetoothDevice sender, object args)
        {
            Debug.WriteLine("lfjkhjklasdfh");
        }

        private void BluetoothConnector_DataReceived(byte[] obj)
        {
            Debug.WriteLine("recvied data");
        }

        private async void BluetoothConnector_ConnectionStatusChanged(BluetoothDevice sender, object args)
        {
            Debug.WriteLine($"Connected Device kjasdfhjkh ID: {sender.DeviceId}, Name: {sender.Name}");
            await ConnectToDevice(sender);


        }

        public async Task SendData(byte[] data)
        {
            if (_socket != null)
            {
                try
                {
                    var writer = new DataWriter(_socket.OutputStream);
                    writer.WriteBytes(data);
                    await writer.StoreAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending data: {ex.Message}");
                }
            }
        }

        public async Task<byte[]> ReceiveData()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            if (_socket != null)
            {
                try
                {
                    // Receive data from the device
                    var inputStream = _socket.InputStream;
                    byte[] buffer = new byte[1024];
                   
                    var bytesRead = await inputStream.ReadAsync(new Buffer(1024),(uint)1024 , InputStreamOptions.Partial);

                    //var b = await bytesRead.ReadAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token);
                    //Debug.WriteLine($"Received data: {b}");
                    using (Stream b = bytesRead.AsStream())
                    {

                        while (b.ReadTimeout > 0)
                        {
                            Debug.WriteLine($"{b.ReadByte()} bytes received");
                        }
                    }
                    return buffer;
                }
                catch (IOException e)
                {
                    Console.WriteLine($"Error receiving data: {e.Message}");
                }
            }

            return null;
        }

        public Task SendData(string data)
        {
            throw new NotImplementedException();
        }
    }
    public class BInputStream : IInputStream
    {
        private bool disposedValue;
        private StreamSocket _socket;
        public BInputStream(StreamSocket socket)
        {
            _socket = socket;
        }

        IAsyncOperationWithProgress<IBuffer, uint> IInputStream.ReadAsync(IBuffer buffer, uint count, InputStreamOptions options)
        {
            if(_socket != null)
            {
               return _socket.InputStream.ReadAsync(buffer, count, options);
            }
            return null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~BInputStream()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
    class Bluebuffer : IBuffer
    {

        byte[] b = new byte[1024];
        public Bluebuffer(ref byte[] bu)
        {
            b = bu;

        }
        public byte[] Get() { return b; }

       
        public uint Capacity => uint.Parse("1024"); 

        public uint Length { get => uint.Parse("1024"); set => value = Length ; }

    }
}
