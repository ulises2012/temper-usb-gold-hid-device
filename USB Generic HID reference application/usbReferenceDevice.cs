﻿//-----------------------------------------------------------------------------
//
//  usbReferenceDevice.cs
//
//  USB Generic HID Communications 3_0_0_0
//
//  A reference test application for the usbGenericHidCommunications library
//  Copyright (C) 2011 Simon Inns
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
//  Web:    http://www.waitingforfriday.com
//  Email:  simon.inns@gmail.com
//
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// The following namespace allows debugging output (when compiled in debug mode)
using System.Diagnostics;

namespace USB_Generic_HID_reference_application
    {
    using usbGenericHidCommunications;

    /// <summary>
    /// This class performs several different tests against the 
    /// reference hardware/firmware to confirm that the USB
    /// communication library is functioning correctly.
    /// 
    /// It also serves as a demonstration of how to use the class
    /// library to perform different types of read and write
    /// operations.
    /// </summary>
    class usbReferenceDevice : usbGenericHidCommunication
        {
        /// <summary>
        /// Class constructor - place any initialisation here
        /// </summary>
        /// <param name="vid"></param>
        /// <param name="pid"></param>
        public usbReferenceDevice(int vid, int pid) : base(vid, pid)
            {
            }


        public bool test1()
            {
            // Test 1 - Send a single write packet to the USB device

            // Declare our output buffer
            Byte[] outputBuffer = new Byte[65];

            // Byte 0 must be set to 0
            outputBuffer[0] = 0;

            // Byte 1 must be set to our command
            outputBuffer[1] = 0x80;

            // Fill the rest of the buffer with known data
            int bufferPointer;
            Byte data = 0;
            for (bufferPointer = 2; bufferPointer < 65; bufferPointer++)
                {
                // We send the numbers 0 to 63 to the device
                outputBuffer[bufferPointer] = data;
                data++;
                }

            // Perform the write command
            bool success;
            success = writeRawReportToDevice(outputBuffer);

            // We can't tell if the device receieved the data ok, we are
            // only indicating that the write was error free.
            return success;
            }

        public bool test2()
            {
            // Test 2 - Send a single write packet to the USB device and get
            // a single packet back
            Debug.WriteLine("Reference Application -> Starting Test 2");

            // Declare our output buffer
            Byte[] outputBuffer = new Byte[65];

            // Declare our input buffer
            Byte[] inputBuffer = new Byte[65];

            // Byte 0 must be set to 0
            outputBuffer[0] = 0;

            // Byte 1 must be set to our command
            outputBuffer[1] = 0x81;

            // Fill the rest of the buffer with known data
            int bufferPointer;
            Byte data = 0;
            for (bufferPointer = 2; bufferPointer < 65; bufferPointer++)
                {
                // We send the numbers 0 to 63 to the device
                outputBuffer[bufferPointer] = data;
                data++;
                }

            // Perform the write command
            bool success;
            success = writeRawReportToDevice(outputBuffer);

            // Only proceed if the write was successful
            if (success)
                {
                // Perform the read
                success = readSingleReportFromDevice(ref inputBuffer);

                // Was the read successful?
                if (!success) return false;

                // Test the received data; we expect 65 bytes, byte[0] is unused
                // bytes 1-64 should be filled with the numbers 0-63
                data = 0;
                for (bufferPointer = 1; bufferPointer < 65; bufferPointer++)
                    {
                    if (inputBuffer[bufferPointer] != data)
                        {
                        Debug.WriteLine("Reference Application -> TEST2: Incorrect data received from device!");
                        return false;
                        }
                    data++;
                    }
                success = true;
                }
            else Debug.WriteLine("Reference Application -> TEST2: Write report to device failed!"); 

            // The data was sent and received ok!
            return success;
            }

        public bool test3()
            {
            // Test 3 - Single packet write, 100 packets read
            Debug.WriteLine("Reference Application -> Starting Test 3");

            // Declare our output buffer
            Byte[] outputBuffer = new Byte[65];

            // Declare our input buffer (this has to be 128 bytes)
            Byte[] inputBuffer = new Byte[128*65];

            // Byte 0 must be set to 0
            outputBuffer[0] = 0;

            // Byte 1 must be set to our command
            outputBuffer[1] = 0x82;

            // Fill the rest of the buffer with known data
            int bufferPointer;
            Byte data = 0;
            for (bufferPointer = 2; bufferPointer < 65; bufferPointer++)
                {
                // We send the numbers 0 to 63 to the device
                outputBuffer[bufferPointer] = data;
                data++;
                }

            // Perform the write command
            bool success;
            success = writeRawReportToDevice(outputBuffer);

            // Only proceed if the write was successful
            if (success)
                {
                Debug.WriteLine("Reference Application -> TEST3: Write command successful, packet sent");

                // Perform the read
                success = readMultipleReportsFromDevice(ref inputBuffer, 128);

                // Was the read successful?
                if (!success)
                    {
                    Debug.WriteLine("Reference Application -> TEST3: Read unsuccessful!}");
                    return false;
                    }

                // Test the received data
                for (int packetCounter = 0; packetCounter < 128; packetCounter++)
                    {
                    // Test the received data; we expect 65 bytes, byte[0] is unused
                    // bytes 1-64 should be filled with the number of the packet
                    for (bufferPointer = 1; bufferPointer < 65; bufferPointer++)
                        {
                        if (inputBuffer[bufferPointer + (packetCounter * 65)] != packetCounter)
                            {
                            Debug.WriteLine(string.Format(
                                "Reference Application -> TEST3: Invalid data - packet number {0}, byte number {1}",
                                packetCounter, bufferPointer));
                            Debug.WriteLine(string.Format(
                                "Reference Application -> TEST3: I expected {0}, but got {1}...",
                                packetCounter, inputBuffer[bufferPointer + (packetCounter * 65)]));
                            return false;
                            }
                        }
                    success = true;
                    }
                }

            // The data was sent and received ok!
            return success;
            }

        public bool test4()
            {
            // Test 4 - 100 packets write, single packet read
            Debug.WriteLine("Reference Application -> Starting Test 4");

            // Declare our output buffer
            Byte[] outputBuffer = new Byte[65];

            // Declare our input buffer
            Byte[] inputBuffer = new Byte[15];

            // Before performing a bulk send to the device we have to send a command
            // packet to let the device know what's about to happen...

            // Byte 0 must be set to 0
            outputBuffer[0] = 0;

            // Byte 1 must be set to our command
            outputBuffer[1] = 0x83;

            // Fill the rest of the buffer with known data
            int bufferPointer;
            Byte data = 0;
            for (bufferPointer = 2; bufferPointer < 65; bufferPointer++)
                {
                // We send the numbers 0 to 63 to the device
                outputBuffer[bufferPointer] = data;
                data++;
                }

            // Perform the write command
            bool success;
            success = writeRawReportToDevice(outputBuffer);

            // If the write was successful we begin the bulk send
            if (success)
                {
                // Now we send 128 packets to the device
                for (int packetCounter = 0; packetCounter < 128; packetCounter++)
                    {
                    // Fill the buffer with meaningful data
                    outputBuffer[0] = 0;
                    for (bufferPointer = 1; bufferPointer < 65; bufferPointer++) outputBuffer[bufferPointer] = (Byte)packetCounter;
                    
                    // Send the packet to the device
                    success = writeRawReportToDevice(outputBuffer);

                    if (!success)
                        {
                        Debug.WriteLine("Reference Application -> TEST4: Bulk send to device failed!");
                        return false;
                        }
                    }
                }
            else Debug.WriteLine("Reference Application -> TEST4: Write report to device failed!");
            readSingleReportFromDevice(ref inputBuffer);
            Debug.Print(inputBuffer.ToString());
            // We can't tell from here if the device received the data ok, you have to check the device status LEDs
            return true;
            }

        public bool test5()
            {
            // Test 5 - Single packet write, timeout on read
            Debug.WriteLine("Reference Application -> Starting Test 5");

            // Declare our output buffer
            Byte[] outputBuffer = new Byte[65];

            // Declare our input buffer
            Byte[] inputBuffer = new Byte[65];

            // Byte 0 must be set to 0
            outputBuffer[0] = 0;

            // Byte 1 must be set to our command
            outputBuffer[1] = 0x84;

            // Fill the rest of the buffer with known data
            int bufferPointer;
            Byte data = 0;
            for (bufferPointer = 2; bufferPointer < 65; bufferPointer++)
                {
                // We send the numbers 0 to 63 to the device
                outputBuffer[bufferPointer] = data;
                data++;
                }

            // Perform the write command
            bool success;
            success = writeRawReportToDevice(outputBuffer);

            // Only proceed if the write was successful
            if (success)
                {
                // Perform the read
                success = readSingleReportFromDevice(ref inputBuffer);

                // Here we expect the read to fail due to a timeout, so failure counts as a success...
                if (!success) return true;
                else
                    {
                    Debug.WriteLine("Reference Application -> TEST5: Write report to device succeeded, but we were expecting a timeout...");
                    return false;
                    }
                }

            // The data was sent and received ok!
            return false;
            }

        // Collect debug information from the device
        public String collectDebug()
            {
            // Collect debug information from USB device
            Debug.WriteLine("Reference Application -> Collecting debug information from device");

            // Declare our output buffer
            //Byte[] outputBuffer = new Byte[15];

            // Declare our input buffer
            Byte[] inputBuffer = new Byte[15];

            // Byte 0 must be set to 0
            //outputBuffer[0] = 0;

            //// Byte 1 must be set to our command
            //outputBuffer[1] = 0x10;

            //// Send the collect debug command
            //writeRawReportToDevice(outputBuffer);

            // Read the response from the device
            readSingleReportFromDevice(ref inputBuffer);

            // Byte 1 contains the number of characters transfered
            //if (inputBuffer[1] == 0) return String.Empty;

            // Convert the Byte array into a string of the correct length
            string s = BitConverter.ToString(inputBuffer) ;
            if (s.Equals("01-00-00-E7-00-00-2A-51-00-00-00-00-FF-28-00"))
                s = "";
            else
                s = s + "\n";

            return s;
            }
        }
    }
