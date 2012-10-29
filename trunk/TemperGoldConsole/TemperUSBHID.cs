using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using usbGenericHidCommunications;

namespace TemperGoldConsole
{
    class TemperUSBHID : usbGenericHidCommunication
    {
        public const int TEMPerGoldVID = 0xC45;
        public const int TEMPerGoldPID = 0x7401;
        const int TEMPerGoldUsagePage = 0xFF00;
        const int TEMPerGoldUsage = 0x1;
        readonly byte[] commandGetCalibration = new byte[] {0, 1, 0x82, 0x77, 1, 0, 0, 0, 0};
        readonly byte[] commandGetVersion = new byte[] { 0, 1, 0x86, 0xFF, 1, 0, 0, 0, 0 };
        readonly byte[] commandReadTemper = new byte[] { 0, 1, 0x80, 0x33, 1, 0, 0, 0, 0 };
        readonly byte[] commandSetCalibration = new byte[] { 0, 1, 0x85, 0xDD, 1, 1, 0, 0, 0 };
        readonly byte[] commandSetValue = new byte[] { 0, 1, 0x81, 0x55, 1, 0, 0, 0, 0 };
        /// <summary>
        /// Class constructor - place any initialisation here
        /// </summary>
        /// <param name="vid"></param>
        /// <param name="pid"></param>
        public TemperUSBHID(int vid, int pid) : base(vid, pid)
            {
            }

        public int getNumTempers()
        {
            //findnum
            List<string> devices = new List<string>();
            //FindCorrectHidDevices(ref devices);
            return devices.Count;
        }

        public double? GetTemperatureDegC()
        {
            Byte[] inputBuffer = new Byte[9];
            bool success = writeRawReportToDevice(commandReadTemper);
            if (success)
            {
                readSingleReportFromDevice(ref inputBuffer);
                int reading = inputBuffer[3] * 16 + inputBuffer[4] / 16;
                if ((reading & 0x800) != 0) reading = reading | 0xF000;
                return (Single)reading * 0.0625;
            }
            else
            {
                return null;
            }
        }

        public string GetVersion()
        {
            Byte[] inputBuffer = new Byte[9];
            bool success = writeRawReportToDevice(commandGetVersion);
            if (success) readSingleReportFromDevice(ref inputBuffer);
            throw new NotImplementedException();
        }

        public int GetCalibration()
        {
            Byte[] inputBuffer = new Byte[9];
            bool success = writeRawReportToDevice(commandGetCalibration);
            if (success) readSingleReportFromDevice(ref inputBuffer);
            return inputBuffer[3] / 16;
        }

        public bool SetCalibration(int calib)
        {
            throw new NotImplementedException();
        }
    }
}
