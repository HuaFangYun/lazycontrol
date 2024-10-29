using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;

namespace lazycontrol.Platforms.Windows
{
    public class BluetoothAdvertiser
    {
        BluetoothLEAdvertisementPublisher publisher;

        public BluetoothAdvertiser()
        {
            publisher = new BluetoothLEAdvertisementPublisher();

            // Create a custom service UUID (e.g., "12345678-1234-5678-1234-567812345678")
            Guid serviceUuid = new Guid("12345678-1234-5678-1234-567812345678");
            BluetoothLEAdvertisement advertisement = publisher.Advertisement;

            // Add the service UUID to the advertisement
            advertisement.ServiceUuids.Add(serviceUuid);


            // Define the advertisement data
            var manufacturerData = new BluetoothLEManufacturerData();
           
            manufacturerData.CompanyId = 0xAA34; // Use your company or app-specific ID
            var writer = new DataWriter();
            writer.WriteByte(1); // App-specific version or identifier
            manufacturerData.Data = writer.DetachBuffer();

            publisher.Advertisement.ManufacturerData.Add(manufacturerData);
        }

        public void StartAdvertising()
        {
            if (publisher.Status == BluetoothLEAdvertisementPublisherStatus.Created || publisher.Status == BluetoothLEAdvertisementPublisherStatus.Stopped)
            {
                publisher?.Start();
                Console.WriteLine("Bluetooth LE advertising started.");
            }
        }

        public void StopAdvertising()
        {
            if (publisher.Status == BluetoothLEAdvertisementPublisherStatus.Started)
            {
                publisher.Stop();
                Console.WriteLine("Bluetooth LE advertising stopped.");
            }
        }
    }
}
