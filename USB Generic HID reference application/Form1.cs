//-----------------------------------------------------------------------------
//
//  Form1.cs
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

//
//  Current reference class library version:
//  usbGenericHidCommunications class library version 2.0.0.0
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace USB_Generic_HID_reference_application
    {
    public partial class Form1 : Form
        {
        /// <summary>
        /// This is a reference application for testing the functionality of the 
        /// usbGenericHidCommunications class library.  It runs a series of 
        /// communication tests against a known USB reference device to determine
        /// if the class library is functioning correctly.
        /// 
        /// You can also use this application as a guide to integrating the 
        /// usbGenericHidCommunications class library into your projects.
        /// 
        /// See http://www,waitingforfriday.com for more detailed documentation.
        /// </summary>
        public Form1()
            {
            InitializeComponent();

            // Create the USB reference device object (passing VID and PID)
            theReferenceUsbDevice = new usbReferenceDevice(0x1B1F, 0xC011);

            // Add a listener for usb events
            theReferenceUsbDevice.usbEvent += new usbReferenceDevice.usbEventsHandler(usbEvent_receiver);

            // Perform an initial search for the target device
            theReferenceUsbDevice.findTargetDevice();
          //  theReferenceUsbDevice.
            }

        // Create an instance of the USB reference device
        private usbReferenceDevice theReferenceUsbDevice;

        // Listener for USB events
        private void usbEvent_receiver(object o, EventArgs e)
            {
            // Check the status of the USB device and update the form accordingly
            if (theReferenceUsbDevice.isDeviceAttached)
                {
                // Device is currently attached

                // Update the status label
                this.usbToolStripStatusLabel.Text = "USB Device is attached";
                // Update the form
                this.button1.Enabled = true;
                this.button2.Enabled = true;
                this.button3.Enabled = true;
                this.button4.Enabled = true;
                this.button5.Enabled = true;
                }
            else
                {
                // Device is currently unattached

                // Update the status label
                this.usbToolStripStatusLabel.Text = "USB Device is detached";

                // Update the form
                this.button1.Enabled = false;
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                this.button4.Enabled = false;
                this.button5.Enabled = false;
                }
            }

        // Test button 1 clicked
        private void button1_Click(object sender, EventArgs e)
            {
            // Perform test 1
            this.test1Label.Text = "Test in progress";
            if (theReferenceUsbDevice.test1()) this.test1Label.Text = "Test passed";
            else this.test1Label.Text = "Test failed";
            }

        // Test button 2 clicked
        private void button2_Click(object sender, EventArgs e)
            {
            // Perform test 2
            this.test2Label.Text = "Test in progress";
            if (theReferenceUsbDevice.test2()) this.test2Label.Text = "Test passed";
            else this.test2Label.Text = "Test failed";
            }

        // Test button 3 clicked
        private void button3_Click(object sender, EventArgs e)
            {
            // Perform test 3
            this.test3Label.Text = "Test in progress";
            if (theReferenceUsbDevice.test3()) this.test3Label.Text = "Test passed";
            else this.test3Label.Text = "Test failed";
            }

        // Test button 4 clicked
        private void button4_Click(object sender, EventArgs e)
            {
            // Perform test 4
            this.test4Label.Text = "Test in progress";
            if (theReferenceUsbDevice.test4()) this.test4Label.Text = "Test passed";
            else this.test4Label.Text = "Test failed";
            }

        // Test button 5 clicked
        private void button5_Click(object sender, EventArgs e)
            {
            // Perform test 5
            this.test5Label.Text = "Test in progress";
            if (theReferenceUsbDevice.test5()) this.test5Label.Text = "Test passed";
            else this.test5Label.Text = "Test failed";
            }

        // Link label to website clicked
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
            {
            // Specify that the link was visited.
            this.linkLabel1.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start("http://www.waitingforfriday.com");
            }

        // Collect debug timer has ticked
        private void debugCollectionTimer_Tick(object sender, EventArgs e)
            {
            String debugString;

            // Only collect debug if the device is attached
            if (theReferenceUsbDevice.isDeviceAttached)
                {
                // Collect the debug information from the device
                debugString = theReferenceUsbDevice.collectDebug();

                // Display the debug information
                if (debugString != String.Empty)
                    {
                    this.debugTextBox.AppendText(debugString);
                    }
                }
            else
                {
                // Clear the debug window
                this.debugTextBox.Clear();
                }
            }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        }
    }
