using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Arduino : MonoBehaviour {

	public string PortName = "";
	public int Baud = 57600;   		// default baud rate
	public int RebootDelay = 3; 	// amount of time to wait after opening connection for arduino to reboot before sending firmata commands
	
	public bool AutoConnect = true;	// connect automatically when the Uniduino instance is created			
	public bool Connected { get; private set; } // true when the device is connected
	
	private static Arduino instance = null;
	public static Arduino Instance {
		get {
			return instance;
		}
	}

	Firmata firmata;
	SerialDevice _serialDevice;

	/// <summary>
	/// Automatically connect to the arduino if properly configured.
	/// </summary>
	void Awake () {		
		
		Debug.Log("Arduino awake");
		if (instance == null) instance = this; // track the first instance that was created as a convenience, but dont preclude multiple uniduino's coexisting
		
		DontDestroyOnLoad(this);

		if (AutoConnect)
		{
			Debug.Log("AutoConnecting...");
			if (PortName == null || PortName.Length == 0 && guessPortName().Length > 0)
			{
				PortName = guessPortName();
			}
			
			Connect();								
		}
		
	}

	/// <summary>
	/// Runs the default serial input processing loop
	/// </summary>
	void Update () {
		if (_serialDevice != null && _serialDevice.IsOpen)
		{
			// process incoming serial messages	
			firmata.processInput();
		}					
	}

	List<Action> setup_queue = new List<Action>();
	private object setup_lock = new object();
	
	/// <summary>
	/// Instruct Uniduino to execute an action only after arduino is connected and not before. 
	/// Use for one-time setup of the board such as setting pinModes and reporting states.
	/// </summary>
	/// <param name='action'> action.</param>
	
	public void Setup(Action action)
	{
		lock(setup_lock)
		{
			if (!Connected)
				setup_queue.Add(action);		
			else
				action();
		}
	}
	/// <summary>
	/// Connect to the device and run any Setup actions you have requested. Called for you if AutoStart is enabled.
	/// </summary>
	public void Connect()
	{
		Debug.Log ("Connectiong to arduino at " + PortName + "...");
		_serialDevice = new SerialDevice (PortName, Baud);
		firmata = new Firmata(_serialDevice, AutoConnect, RebootDelay);

//		Firmata.VersionDataReceived += delegate(int majorVersion, int minorVersion) 
//		{
//			StartCoroutine(setup_delay());				
//		};
//
//		firmata.reportVersion(); // some boards (like the micro) do not send the version right away for some reason. perhaps a timing issue.		
	}
	
	IEnumerator setup_delay (){	
		yield return new WaitForSeconds(RebootDelay);
		Debug.Log("Version data received, running setup commands");			
		lock(setup_lock)
		{
			Connected = true;	
			foreach (var a in setup_queue)
			{
				a();
			}
			setup_queue.Clear();
		}
	}
	
	
	public void Disconnect()
	{
		Connected = false;
		firmata.Close ();
		
	}
	
	void OnDestroy()
	{				
		Disconnect();
	}
	
	
	// Static Helpers	
	public string guessPortName()
	{		
		switch (Application.platform)
		{
		case RuntimePlatform.OSXPlayer:
		case RuntimePlatform.OSXEditor:
		case RuntimePlatform.OSXDashboardPlayer:
		case RuntimePlatform.LinuxPlayer:
			return guessPortNameUnix();

		default: 
			return guessPortNameWindows();
		}
	}
	
	public string guessPortNameWindows()
	{
		var devices = _serialDevice.GetPortNames();
		
		if (devices.Length == 0) // 
		{
			return "COM3"; // probably right 50% of the time		
		} else
			return devices[0];				
	}
	
	public string guessPortNameUnix()
	{			
		var devices = _serialDevice.GetPortNames();
		
		if (devices.Length ==0) // try manual enumeration
		{
			devices = System.IO.Directory.GetFiles("/dev/");		
		}
		string dev = ""; ;			
		foreach (var d in devices)
		{				
			if (d.StartsWith("/dev/tty.usb") || d.StartsWith("/dev/ttyUSB"))
			{
				dev = d;
				Debug.Log("Guessing that arduino is device " + dev);
				break;
			}
		}		
		return dev;		
	}

}
