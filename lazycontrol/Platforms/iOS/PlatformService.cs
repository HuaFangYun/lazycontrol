using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lazycontrol.Platforms.iOS
{
    public class PlatformService : IPlatformService
    {
        public string GetPlatformMessage()
        {
            return "iOS";
        }
    }
}
