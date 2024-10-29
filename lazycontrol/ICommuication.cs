using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lazycontrol
{
    public interface ICommuication
    {
        public event Action<byte[]> DataReceived;
        public void Test(string odf);
    }
}
