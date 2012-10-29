using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemperGoldConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new TemperUSBHID(TemperUSBHID.TEMPerGoldVID,TemperUSBHID.TEMPerGoldPID);
            x.findTargetDevice(1);
            //var z = x.getNumTempers();
            var y = x.GetTemperatureDegC();
            y = y + 1;// z;
        }
    }
}
