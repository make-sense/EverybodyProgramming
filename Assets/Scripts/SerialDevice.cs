using UnityEngine;
using System.Collections;
#if UNITY_STANDALONE
using System.IO.Ports;
#endif

public class SerialDevice {

	#if UNITY_STANDALONE
	SerialPort _serialPort = null;
	#endif

	public SerialDevice(string deviceName, int baudRate) {
		#if UNITY_STANDALONE
		if (deviceName.Length > 0) 
		{
			if (_serialPort != null)
			{
				_serialPort.Dispose ();
				_serialPort = null;
			}
			_serialPort = new SerialPort(deviceName, baudRate, Parity.None, 8, StopBits.One);
			_serialPort.DtrEnable = true; // win32 hack to try to get DataReceived event to fire
			_serialPort.RtsEnable = true; 
			_serialPort.PortName = deviceName;
			_serialPort.BaudRate = baudRate;
			
			_serialPort.DataBits = 8;
			_serialPort.Parity = Parity.None;
			_serialPort.StopBits = StopBits.One;
			_serialPort.ReadTimeout = 1; // since on windows we *cannot* have a separate read thread
			_serialPort.WriteTimeout = 1000;
		}
		#endif
	}

	public void Open() {
		#if UNITY_STANDALONE
		if (_serialPort != null)
		{
			if (_serialPort.IsOpen)
				_serialPort.Close ();
			_serialPort.Open ();
		}
		#endif
	}

	public void Close() {
		#if UNITY_STANDALONE
		if (_serialPort != null)
			_serialPort.Close ();
		#endif
	}

	public int Read(byte[] buffer, int offset, int count) {
		#if UNITY_STANDALONE
		if (_serialPort != null)
			return _serialPort.Read (buffer, offset, count);
		else
			return 0;
		#else
		return 0;
		#endif
	}

	public int ReadByte() {
		#if UNITY_STANDALONE
		if (_serialPort != null)
			return _serialPort.ReadByte ();
		else
			return 0;
		#else
		return 0;
		#endif
	}

	public void Write(byte[] buffer, int offset, int count) {
		#if UNITY_STANDALONE
		if (_serialPort != null)
			_serialPort.Write (buffer, offset, count);
		#endif
	}

	public static string[] GetPortNames() {
		#if UNITY_STANDALONE
		return SerialPort.GetPortNames ();
		#else
		return null;
		#endif
	}

	public int BytesToRead 
	{
		get 
		{
			#if UNITY_STANDALONE
			if (_serialPort != null)
				return _serialPort.BytesToRead;
			else
				return 0;
			#else
			return 0;
			#endif
		}
	}

	public bool IsOpen
	{
		get
		{
			#if UNITY_STANDALONE
			if (_serialPort != null)
				return _serialPort.IsOpen;
			else
				return false;
			#else
			return false;
			#endif
		}
	}
}
