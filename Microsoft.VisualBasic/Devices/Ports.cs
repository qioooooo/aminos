using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Security.Permissions;

namespace Microsoft.VisualBasic.Devices
{
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class Ports
	{
		public SerialPort OpenSerialPort(string portName)
		{
			SerialPort serialPort = new SerialPort(portName);
			serialPort.Open();
			return serialPort;
		}

		public SerialPort OpenSerialPort(string portName, int baudRate)
		{
			SerialPort serialPort = new SerialPort(portName, baudRate);
			serialPort.Open();
			return serialPort;
		}

		public SerialPort OpenSerialPort(string portName, int baudRate, Parity parity)
		{
			SerialPort serialPort = new SerialPort(portName, baudRate, parity);
			serialPort.Open();
			return serialPort;
		}

		public SerialPort OpenSerialPort(string portName, int baudRate, Parity parity, int dataBits)
		{
			SerialPort serialPort = new SerialPort(portName, baudRate, parity, dataBits);
			serialPort.Open();
			return serialPort;
		}

		public SerialPort OpenSerialPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
		{
			SerialPort serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
			serialPort.Open();
			return serialPort;
		}

		public ReadOnlyCollection<string> SerialPortNames
		{
			get
			{
				string[] portNames = SerialPort.GetPortNames();
				List<string> list = new List<string>();
				foreach (string text in portNames)
				{
					list.Add(text);
				}
				return new ReadOnlyCollection<string>(list);
			}
		}
	}
}
