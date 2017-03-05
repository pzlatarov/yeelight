using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Pzlatarov.Yeelight;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var bulbs = YeelightDiscovery.Discover();
           
           // bulbs[0].RunMethod(20, "set_power", new object[] { "on", "smooth", 500 });
           // Thread.Sleep(5000);
           // bulbs[0].RunMethod(20, "set_power", new object[] { "off", "smooth", 500 });
            bulbs[0].PowerState = Yeelight.PowerStateType.On;
            Thread.Sleep(2000);
            bulbs[0].PowerState = Yeelight.PowerStateType.Off;
            Thread.Sleep(2000);
            bulbs[0].PowerState = Yeelight.PowerStateType.On;
            Thread.Sleep(2000);
            bulbs[0].PowerState = Yeelight.PowerStateType.Off;
            Thread.Sleep(2000);
            bulbs[0].PowerState = Yeelight.PowerStateType.On;
            Thread.Sleep(2000);
            bulbs[0].PowerState = Yeelight.PowerStateType.Off;


        }
    }
}
