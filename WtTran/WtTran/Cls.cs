using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.IO;

namespace WtTran
{
    public class clsGetWt
    {
        SerialPort _serialPort;
        string readStr;
        string wet;
        public double Weight;

        public void SerialPortInitialize(int _baudRate, string _portName)
        {
            string val = ""; //GetPortSetting();
            if (val == "")
            {
                //FOR TESTING
                //_serialPort = new SerialPort("COM1", 19200, Parity.None, 8, StopBits.One);
                _serialPort = new SerialPort(_portName, _baudRate,Parity.None, 8, StopBits.One);
                //_serialPort = new SerialPort(Portname, baudrate, Parity.None, databits, StopBits.One);
                _serialPort.Handshake = Handshake.None;
                //_serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                _serialPort.ReadTimeout = 500;
                _serialPort.WriteTimeout = 500;
                if (!_serialPort.IsOpen)
                    _serialPort.Open();
            }
            else
            {
                //lblerrormsg.Text = "Set Softcontrol Values";
            }
        }

        public double GetWt(int _baudRate, string _portName)
        {
            Weight = 0;
            try
            {
                wet = "";
                SerialPortInitialize(_baudRate, _portName);
                if (!_serialPort.IsOpen)
                    _serialPort.Open();
                Thread.Sleep(500);                
                string data = _serialPort.ReadExisting();                
                for (int cnt = 1; cnt <= 10; cnt++)
                {
                    readStr = data.ToUpper();
                    if (readStr.Contains("."))
                        goto a;
                }
            a:
                string[] wt = readStr.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                int Index = wt.Length - 1;
                if (!wt[Index].Contains(".")) { Index -= 2; }

                foreach (Char c in wt[Index])
                {
                    if (c == ',') continue;
                    if (Char.IsPunctuation(c)) wet += c;
                    if (Char.IsNumber(c)) wet += c;
                }

                Weight = Convert.ToDouble(wet.Trim());
                return Weight;
            }
            catch (Exception e1)
            {
                //MessageBox.Show("Weight Machine Connection Lost...");
                return Weight;
            }
            finally
            {
                _serialPort.Close();
            }
        }


    }
}
