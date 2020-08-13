/**
  ******************************************************************************
  * @file    USBterminal.sc
  * @author  TOMAS
  * @version V2.4.0t
  * @date    8-June-2015
  * @brief   Header for usbd_comp file.
  ******************************************************************************
  * COPYRIGHT(c) 2015 STMicroelectronics
  *
  * Redistribution and use in source and binary forms, with or without modification,
  * are permitted provided that the following conditions are met:
  * 1. Redistributions of source code must retain the above copyright notice,
  * this list of conditions and the following disclaimer.
  * 2. Redistributions in binary form must reproduce the above copyright notice,
  * this list of conditions and the following disclaimer in the documentation
  * and/or other materials provided with the distribution.
  * 3. Neither the name of STMicroelectronics nor the names of its contributors
  * may be used to endorse or promote products derived from this software
  * without specific prior written permission.
  *
  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
  * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
  * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
  * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
  * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
  * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
  * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
  * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
  * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
  * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
  *
  ******************************************************************************
**/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using LibUsbDotNet;
using LibUsbDotNet.Main;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        const int numBytes = 512;
        volatile int numberOfDataRecieved;
        int timer_done = 0;
        int connect_time = 0;
        bool commands_not_sent = true;
        bool oscilloscope_function_on = false;
        uint rec_count = 0;
        uint rec_count2 = 0;

        static UsbDevice MyUsbDevice;
        static UsbDeviceFinder MyUsbFinder = new UsbDeviceFinder(1155, 0xC1B0);
        ErrorCode ec = ErrorCode.None;
        UsbEndpointReader reader1;
        UsbEndpointWriter writer1;

        Thread USB_receive;
        Thread USB_connect;

        byte[] readBuffer1;
        byte[] writeBuffer1;
        byte[] reciveBuffer;
        double[] chart_os;
        double[] chart_os2 = { 1,2,3,2,1,0,1,2 };
        int reciveCounter;

        volatile bool running = true;
        volatile bool device_connected = false;
        string newLine = Environment.NewLine;

        public Form1()
        {
            InitializeComponent();
            readBuffer1 = new byte[numBytes];
            writeBuffer1 = new byte[numBytes];
            chart_os = new double[16];
            System.Threading.Thread.Sleep(1000);
            StatusLabel2.Text = "Text1";
            System.Threading.Thread.Sleep(1000);
            StatusLabel2.Text = "Text2";
            toolStripStatusLabel1.Text = "Waiting for device connection";



            Random rnd = new Random();
            //Chart chart1 = new Chart();
            chart1.Series.Add("duck");
            chart1.Series["duck"].Enabled = true;
            chart1.Visible = true;


            chart1.Series["duck"].ChartArea = "ChartArea1";
            chart1.Series["duck"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series["duck"].Legend = "Legend1";
            //chart1.Series["duck"].Name = "Series_d";


            for (int q = 0; q < 100; q++)
            {
                int second = rnd.Next(0, 10);
                chart1.Series["duck"].Points.AddXY(q, second);
            }
            chart1.Show();
            Controls.Add(chart1);
            chart1.Show();


        }



        private void init_Click(object sender, EventArgs e)
        {
            if (connectButton.Text == "Start")
            {
                running = true;
                USB_connect = new Thread(USB_connection);
                USB_connect.Start();
                connectButton.Text = "Stop";
            }
            else if (connectButton.Text == "Stop")
            {
                running = false;
                device_connected = false;
                button1Enable(false);
                Thread.Sleep(100);
                connectButton.Text = "Start";
                toolStripStatusLabel1.Text = "Stopped";
            }

        }
        private void USB_connection()
        {
            while (running == true)
            {
                if (device_connected == false)
                {
                    while (MyUsbDevice == null && running == true)
                    {
                        Thread.Sleep(200);
                        toolStripStatusLabel1.Text = "Waiting for device connection";
                        MyUsbDevice = UsbDevice.OpenUsbDevice(MyUsbFinder);
                    }
                    if (running == true)
                    {
                        device_connected = true;
                        USB_init();
                        toolStripStatusLabel1.Text = "Device connected";
                    }
                }
                Thread.Sleep(100);
            }
        }

        private void USB_init()
        {
            IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
            if (!ReferenceEquals(wholeUsbDevice, null))
            {
                // This is a "whole" USB device. Before it can be used, 
                // the desired configuration and interface must be selected.

                // Select config #1
                wholeUsbDevice.SetConfiguration(1);

                // Claim interface #0.
                wholeUsbDevice.ClaimInterface(0);
            }
            reader1 = MyUsbDevice.OpenEndpointReader(ReadEndpointID.Ep01);
            writer1 = MyUsbDevice.OpenEndpointWriter(WriteEndpointID.Ep01);

            // Setup trigger

            USB_receive = new Thread(USBReceive);
            // open read endpoint 1.

            USB_receive.Start();

            button1Enable(true);
        }

        private void USBReceive()
        {
            try
            {
                while (device_connected && ((ec == ErrorCode.None) || (ec == ErrorCode.IoTimedOut)))
                {
                    int bytesRead = 0;
                    ec = reader1.Transfer(readBuffer1, 0, numBytes, 500, out bytesRead);
                    numberOfDataRecieved = bytesRead;
                    lock (USB_receive)
                    {
                        if (running && ec == ErrorCode.None)
                        {
                            this.BeginInvoke(new EventHandler(DoUpdate));
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine((ec != ErrorCode.None ? ec + ":" : String.Empty) + ex.Message);
            }
            finally
            {
                if (running == true)
                {
                    Thread.Sleep(200);

                    device_connected = false;


                    button1Enable(false);

                    disconnect();

                    ec = ErrorCode.None;

                    toolStripStatusLabel1.Text = "Device disconnected";
                }
            }
        }

        public void DoUpdate(object sender, System.EventArgs e) => reciveCounter += readBuffer1.Length;

        public void DoUpdate22(object sender, System.EventArgs e)
        {
            byte[] s = readBuffer1;
            if (s[0] == 0xff && s[1] == 0xff)
            {
                terminalBox.AppendText("Time on device: ");
                terminalBox.AppendText((1900 + s[4]).ToString());
                terminalBox.AppendText(".");
                terminalBox.AppendText(s[5].ToString());
                terminalBox.AppendText(".");
                terminalBox.AppendText(s[7].ToString());
                terminalBox.AppendText(" ");
                terminalBox.AppendText(s[8].ToString());
                terminalBox.AppendText(":");
                terminalBox.AppendText(s[9].ToString());
                terminalBox.AppendText(":");
                terminalBox.AppendText(s[10].ToString());
                terminalBox.AppendText(newLine);
            }
            else if (s[0] == 0xff && s[1] == 0xfe)
            {
                rec_count++;
                //terminalBox.AppendText("ADC voltage: ");
                //double get_num = ((s[3] * 256) + s[2]);
                //double voltage = 3.3*(get_num / 4096);
                //long count = ((((( s[504] * 256 ) + s[503] ) * 256 ) + s[502] ) * 256 ) + s[501] ;
                //terminalBox.AppendText(voltage.ToString() + " ");
                //get_num = ((s[17] * 256) + s[16]);
                //voltage = 3.3 * (get_num / 4096);
                //terminalBox.AppendText(voltage.ToString() + newLine);
                //for (int i = 2; i <= 500; i++)
                //{
                //    terminalBox.AppendText(String.Format(" 0x{0:X}", s[i]));
                //}
                if ((rec_count % 1000) == 3)
                {
                    terminalBox.AppendText(DateTime.Now.ToString("yyyy MM dd dddd HH mm ss"));
                    terminalBox.AppendText(newLine);
                }
                //terminalBox.AppendText("Number of samples: " + count.ToString() + newLine);
            }
            else
            {
                rec_count2 ++;
                if ((rec_count2 % 1000) == 3)
                    terminalBox.AppendText("No match ");
                //foreach (byte by in s)
                //{
                //    if (by < 0x1b && by > 0x2f)
                //    {
                //        terminalBox.AppendText(Convert.ToChar(by).ToString());
                //    }
                //    else
                //    {
                //        terminalBox.AppendText(" 0x");
                //        terminalBox.AppendText(String.Format("{0:X}", by));
                //    }
                //}
            }
            terminalBox.AppendText("\n");
        }

        private void disconnect()
        {

            if (MyUsbDevice != null)
            {
                if (MyUsbDevice.IsOpen)
                {
                    // If this is a "whole" usb device (libusb-win32, linux libusb-1.0)
                    // it exposes an IUsbDevice interface. If not (WinUSB) the 
                    // 'wholeUsbDevice' variable will be null indicating this is 
                    // an interface of a device; it does not require or support 
                    // configuration and interface selection.
                    IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
                    if (!ReferenceEquals(wholeUsbDevice, null))
                    {
                        // Release interface #0.
                        wholeUsbDevice.ReleaseInterface(0);
                    }

                    MyUsbDevice.Close();
                }
                MyUsbDevice = null;

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            running = false;
            device_connected = false;
            UsbDevice.Exit();

            Thread.Sleep(100);
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int bytesWritten = 0;
            ASCIIEncoding.ASCII.GetBytes(sendTextBox1.Text, 0, sendTextBox1.Text.Length, writeBuffer1, 0);
            writer1.Transfer(writeBuffer1, 0, sendTextBox1.Text.Length, 5000, out bytesWritten);
        }

        private void button1Enable(bool val)
        {
            if (sendData1.InvokeRequired)
            {
                Invoke((MethodInvoker)(() => button1Enable(val)));
            }
            else
            {
                sendData1.Enabled = val;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            StatusLabel2.Text = "Text3 " + timer_done;
            timer_done++;
            if (device_connected == false)
            {
                connect_time = 0;
                MyUsbDevice = UsbDevice.OpenUsbDevice(MyUsbFinder);
                if (MyUsbDevice != null)
                {
                    device_connected = true;
                    USB_init();
                    toolStripStatusLabel1.Text = "Device connected";
                }
            }
            else {
                connect_time++;
                if (connect_time > 50 && commands_not_sent) {
                    commands_not_sent = false;
                    string text1 = DateTime.Now.ToString("yyyy MM dd dddd HH mm ss");
                    string[] words = text1.Split(' ');
                    writeBuffer1[0] = 51;
                    int temp_int = Convert.ToInt32(words[0]) - 1900;
                    writeBuffer1[4] = Convert.ToByte(Convert.ToInt32(words[0]) - 1900);
                    writeBuffer1[5] = Convert.ToByte(Convert.ToInt32(words[1]));
                    writeBuffer1[6] = Convert.ToByte((int)DateTime.Now.DayOfWeek);
                    writeBuffer1[7] = Convert.ToByte(Convert.ToInt32(words[2]));
                    writeBuffer1[8] = Convert.ToByte(Convert.ToInt32(words[4]));
                    writeBuffer1[9] = Convert.ToByte(Convert.ToInt32(words[5]));
                    writeBuffer1[10] = Convert.ToByte(Convert.ToInt32(words[6]));
                    temp_int = 0;
                    writer1.Transfer(writeBuffer1, 0, 12, 5000, out temp_int);
                }
            }
        }

        private void ClearBtn_Click_1(object sender, EventArgs e)
        {
            terminalBox.Clear();
        }

        private void Send_time_Click(object sender, EventArgs e)
        {
            string text1 = DateTime.Now.ToString("yyyy MM dd dddd HH mm ss");
            string[] words = text1.Split(' ');
            writeBuffer1[0] = 51;
            int temp_int = Convert.ToInt32(words[0]) - 1900;
            writeBuffer1[4] = Convert.ToByte(Convert.ToInt32(words[0]) - 1900);
            writeBuffer1[5] = Convert.ToByte(Convert.ToInt32(words[1]));
            writeBuffer1[6] = Convert.ToByte((int)DateTime.Now.DayOfWeek);
            writeBuffer1[7] = Convert.ToByte(Convert.ToInt32(words[2]));
            writeBuffer1[8] = Convert.ToByte(Convert.ToInt32(words[4]));
            writeBuffer1[9] = Convert.ToByte(Convert.ToInt32(words[5]));
            writeBuffer1[10] = Convert.ToByte(Convert.ToInt32(words[6]));
            temp_int = 0;
            writer1.Transfer(writeBuffer1, 0, 12, 5000, out temp_int);
            terminalBox.AppendText(DateTime.Now.ToString("yyyy MM dd dddd HH mm ss"));
            terminalBox.AppendText("\n");
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (oscilloscope_function_on)
            {
                writeBuffer1[0] = 53;
                int temp_int = 0;
                writer1.Transfer(writeBuffer1, 0, 12, 5000, out temp_int);
            }
            //terminalBox.AppendText(reciveCounter + newLine);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            oscilloscope_function_on = !oscilloscope_function_on;
            terminalBox.AppendText("Button pressed ... "+newLine);
        }
    }
}
