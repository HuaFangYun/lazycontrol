using lazycontrol;
using lazycontrol.Platforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace lazycontrol
{
    public interface IBluetoothConnector
    {
        public event Action<byte[]> DataReceived;

        public event Action<Object> IsConnect;

        public event Action<Object> bs;

        Task DisplayConnectedBluetoothDevices();
        Task<bool> ConnectToDevice(Object deviceAddress);
        Task SendData(byte[] data);
        Task SendData(string data);
        Task<byte[]> ReceiveData();
        Task DisconnectDevice();
    }
}
