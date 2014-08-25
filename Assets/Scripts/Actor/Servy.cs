using UnityEngine;
using System.Collections;
using Uniduino;

public class Servy : Actor {
	
	Arduino arduino;
	private bool _configured = false;
	public int pin = -1;
	
	public void SetAngle(int angle)
	{
		Debug.Log ("SetAngle");
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
		arduino.pinMode(pin, PinMode.SERVO);
		_configured = true;
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
}
