using UnityEngine;
using System.Collections;
using Uniduino;

public class DcMotor : Actor {
	Arduino arduino;
	private bool _configured = false;
	public int pin = -1;
	
	public void SetPower(string power)
	{
		if (_configured)
		{
			int _power = System.Convert.ToInt32(power);
			arduino.analogWrite(pin, _power);
			Debug.Log ("SetAngle:" + power);
		}
	}
	
	public void AttachPin(int p)
	{
		UILabel uiLabel = this.GetComponentInChildren<UILabel> () as UILabel;
		
		pin = p;
		if (pin >= 0) 
		{
			_configured = false;
			arduino.Setup (ConfigurePin);
			uiLabel.text = "D" + pin.ToString ();
		} 
		else 
		{
			uiLabel.text = "";
		}
	}
	
	void ConfigurePin ()
	{
		arduino.pinMode(pin, PinMode.PWM);
		_configured = true;
		Debug.Log ("DcMotor:Configured with " + pin.ToString ());
	}
	
	// Use this for initialization
	void Start () {
		base.Start ();
		ActorManager.Instance.Add (this);
		arduino = Arduino.global;
	}
	
	public override void Refresh () {
	}
	
	// Update is called once per frame
	void Update () {	
	}
	
	void OnPress (bool isPressed) 
	{
		Debug.Log ("DcMotor OnPress");
		PropertyManager.Instance.ShowProperty (Guid);
	}
}
