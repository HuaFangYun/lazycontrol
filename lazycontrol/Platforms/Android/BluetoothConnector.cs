using Android.Bluetooth;
using Android.OS;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using InTheHand.Net.Bluetooth;
using Java.Nio;
using lazycontrol.Platforms.Android;


[assembly: Dependency(typeof(BluetoothConnector))]
namespace lazycontrol.Platforms.Android
{
    public class BluetoothConnector : IBluetoothConnector
    {
        private BluetoothAdapter _bluetoothAdapter;
        private BluetoothSocket _bluetoothSocket;
        private CancellationTokenSource _cancellationTokenSource;
        private BluetoothClient _client;
        private BluetoothEndPoint _endPoint;

        public BluetoothConnector()
        {
            _bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            //_client = new BluetoothClient();
            
        }

        public event Action<byte[]> DataReceived;
        public event Action<Object> IsConnect;
        public event Action<object> bs;

        public async Task<bool> ConnectToDevice(Object devicesa)
        {   
            var deviceAddress = devicesa as BluetoothDevice;
            
            if (_bluetoothAdapter == null || !_bluetoothAdapter.IsEnabled)
            {
                Console.WriteLine("Bluetooth is disabled or not supported.");
                return false;
            }

            try
            {
                // Get the Bluetooth device using its address
                var device = _bluetoothAdapter.GetRemoteDevice(deviceAddress?.Address);

                ParcelUuid[] parcelUuid = device?.GetUuids();
                string uid = "";
                foreach(ParcelUuid parcel in parcelUuid)
                {
                    uid = parcel.ToString();
                    
                }
                Guid guid = Guid.Parse(uid);

                //_endPoint = new BluetoothEndPoint(BluetoothAddress.Parse(deviceAddress.Address),guid);
                // Use the device's UUID for establishing a connection
                var uuid = Java.Util.UUID.FromString(uid); // Standard UUID for SPP
                _bluetoothSocket = device?.CreateRfcommSocketToServiceRecord(uuid);

                // Cancel discovery for faster connection
                _bluetoothAdapter.CancelDiscovery();

                //Connect to the device
                _bluetoothSocket?.Connect();

                if (_bluetoothSocket.IsConnected)
                {
                    Message("Connected to device.");
                    Message($"Connected to device{deviceAddress?.Name}");
                    IsConnect.Invoke(true);
                    StartListeningForData();
                    return true;
                }
            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.WriteLine($"Error connecting to device: {e.Message}");
            }

            return false;
        }
        private async void Message(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string text = message;
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);


        }
        private void StartListeningForData()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(async () =>
            {
                try
                {
                    var inputStream = _bluetoothSocket.InputStream;
                    byte[] buffer = new byte[1024];

                    while (!_cancellationTokenSource.IsCancellationRequested)
                    {
                        // Check if data is available and read it
                        int bytesRead = await inputStream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesRead > 0)
                        {
                            byte[] receivedData = buffer.Take(bytesRead).ToArray();

                            // Trigger the event when data is received
                            DataReceived?.Invoke(receivedData);
                        }
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine($"Error receiving data: {e.Message}");
                }
            });
        }
        public async Task DisconnectDevice()
        {
            if (_bluetoothSocket != null)
            {
                try
                {
                    _bluetoothSocket.Close();
                    Console.WriteLine("Disconnected from device.");
                }
                catch (IOException e)
                {
                    Console.WriteLine($"Error disconnecting: {e.Message}");
                }
            }
        }

        public async Task<byte[]> ReceiveData()
        {
            if (_bluetoothSocket != null && _bluetoothSocket.IsConnected)
            {
                try
                {
                    // Receive data from the device
                    var inputStream = _bluetoothSocket.InputStream;
                    byte[] buffer = new byte[1024];
                    int bytesRead = await inputStream.ReadAsync(buffer, 0, buffer.Length);
                    DataReceived?.Invoke(buffer);
                    return buffer.Take(bytesRead).ToArray();
                }
                catch (IOException e)
                {
                    Console.WriteLine($"Error receiving data: {e.Message}");
                }
            }

            return null;
        }
    

        public async Task SendData(byte[] data)
        {
            Stream stream = _client.GetStream();
            
            if (_bluetoothSocket != null && _bluetoothSocket.IsConnected)
            {
                try
                {

                    // Send data to the device
                    //var outputStream = _bluetoothSocket.OutputStream;
                    //await outputStream.WriteAsync(data, 0, data.Length);
                    Console.WriteLine("Data sent to the device.");
                }
                catch (IOException e)
                {
                    Console.WriteLine($"Error sending data: {e.Message}");
                }
            }
        }
        public async Task SendData(string data)
        {
            if (_bluetoothSocket != null && _bluetoothSocket.IsConnected)
            {
                try
                {
                    bs.Invoke(_bluetoothSocket.OutputStream);
                    // Send data to the device
                    //var outputStream = _bluetoothSocket.OutputStream;
                    
                    //await outputStream?.WriteAsync(Encoding.Default.GetBytes(data), 0, data.Length);
                    //DataReceived?.Invoke(Encoding.Default.GetBytes(data));
                    Console.WriteLine("Data sent to the device.");
                }
                catch (IOException e)
                {
                    Console.WriteLine($"Error sending data: {e.Message}");
                }
            }
        }

        public Task<bool> ConnectToDevice(string deviceAddress)
        {
            throw new NotImplementedException();
        }

        public Task DisplayConnectedBluetoothDevices()
        {
            throw new NotImplementedException();
        }
    }
   
}
