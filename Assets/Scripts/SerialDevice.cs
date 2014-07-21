using UnityEngine;
using System.Collections;
#if UNITY_STANDALONE
using System.IO.Ports;
#endif

public class SerialDevice {

	#if UNITY_STANDALONE
	SerialPort _serialPort;
	#endif

	public SerialDevice(string deviceName, int baudRate) {
		#if UNITY_STANDALONE
		_serialPort = new SerialPort(deviceName, baudRate, Parity.None, 8, StopBits.One);
		#endif
	}

	public void Open() {
		#if UNITY_STANDALONE
		_serialPort.Open ();
		#endif
	}

	public void Close() {
		#if UNITY_STANDALONE
		_serialPort.Close ();
		#endif
	}

	public int Read(byte[] buffer, int offset, int count) {
		#if UNITY_STANDALONE
		return _serialPort.Read (buffer, offset, count);
		#else
		return 0;
		#endif
	}

	public int ReadByte() {
		#if UNITY_STANDALONE
		return _serialPort.ReadByte ();
		#else
		return 0;
		#endif
	}

	public void Write(byte[] buffer, int offset, int count) {
		#if UNITY_STANDALONE
		_serialPort.Write (buffer, offset, count);
		#endif
	}

	public string[] GetPortNames() {
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
			return _serialPort.BytesToRead;
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
			return _serialPort.IsOpen;
			#else
			return false;
			#endif
		}
	}
}
