using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lazycontrol
{
    public class Communication : ICommuication
    {
        public event Action<byte[]> DataReceived;
        private string thisstring;

        public void Test(string o)
        {
            Debug.WriteLine(o);
            thisstring = o;
            DataReceived?.Invoke(Encoding.Default.GetBytes(o));
        }
    }
}
