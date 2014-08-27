using UnityEngine;
using System.Collections;
using Uniduino;

public class Analog : Actor {

	Arduino arduino;
	private bool _configured = false;
	
	public int pin = -1;

	int analogValue;

	public bool IsLess (string value)
	{
		int cmp = System.Convert.ToInt32 (value);
		if (analogValue < cmp)
			return true;
		return false;
	}

	public bool IsLessEqual (string value)
	{
		int cmp = System.Convert.ToInt32 (value);
		if (analogValue <= cmp)
			return true;
		return false;
	}
	
	public bool IsEqual (string value)
	{
		int cmp = System.Convert.ToInt32 (value);
		if (analogValue == cmp)
			return true;
		return false;
	}
	
	public bool IsLargerEqual (string value)
	{
		int cmp = System.Convert.ToInt32 (value);
		if (analogValue >= cmp)
			return true;
		return false;
	}
	
	public bool IsLarger (string value)
	{
		int cmp = System.Convert.ToInt32 (value);
		if (analogValue > cmp)
			return true;
		return false;
	}
	
	public void AttachPin(int p)
	{
		UILabel uiLabel = this.GetComponentInChildren<UILabel> () as UILabel;
		
		pin = p;
		if (pin >= 0) 
		{
			_configured = false;
			arduino.Setup (ConfigurePin);
			uiLabel.text = "A" + pin.ToString ();
		} 
		else 
		{
			uiLabel.text = "";
		}
	}
	
	void ConfigurePin ()
	{
		arduino.pinMode(pin, PinMode.ANALOG);
		arduino.reportAnalog(pin, 1);
		_configured = true;
	}
	
	public void Start () 
	{
		base.Start ();
		ActorManager.Instance.Add (this);
		arduino = Arduino.global;
	}
	
	public override void Refresh () 
	{
		if (_configured) 
		{
			analogValue = arduino.analogRead(pin);
			Debug.Log (analogValue.ToString ());
		}
	}
	
	void OnPress (bool isPressed) 
	{
		PropertyManager.Instance.ShowProperty (Guid);
	}
}
