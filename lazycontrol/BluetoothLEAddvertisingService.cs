using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lazycontrol
{
    public  partial class BluetoothLEAddvertisingService
    {
#if WINDOWS        
        Platforms.Windows.BluetoothAdvertiser advertiser = new Platforms.Windows.BluetoothAdvertiser();

        public async Task StartAdvertising()
        {
            advertiser.StartAdvertising();
        }

        public void StopAdvertising()
        {
            advertiser.StopAdvertising();
        }
#endif
    }

}
