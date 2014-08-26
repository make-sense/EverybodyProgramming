using UnityEngine;
using System.Collections;
using Uniduino;

public class Analog : Actor {

	Arduino arduino;
	private bool _configured = false;
	
	public int pin = -1;

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
			int value = arduino.analogRead(pin);
		}
	}
	
	void OnPress (bool isPressed) 
	{
		PropertyManager.Instance.ShowProperty (Guid);
	}
}
