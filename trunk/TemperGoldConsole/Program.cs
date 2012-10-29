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
            string retval = DateTime.Now.ToString();
            for(int i = 0;i<10;i++)
            {
                x.findTargetDevice(i);
                double? temp = x.GetTemperatureDegC();
                if (temp != null)
                {
                    retval += "," + temp.ToString();
                }
            }
            Console.WriteLine(retval);
        }
    }
}
