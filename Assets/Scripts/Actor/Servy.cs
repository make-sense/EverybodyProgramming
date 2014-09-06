using UnityEngine;
using System.Collections;
using Uniduino;

public class Servy : Actor {
	
	Arduino arduino;
	private bool _configured = false;
	public int pin = -1;
	public Transform Horn;
	public float Speed = 1000f;
	int targetAngle = 90;
	
	public void SetAngle(string angle_str)
	{
		targetAngle = System.Convert.ToInt32(angle_str);
		if (_configured)
		{
			arduino.analogWrite(pin, targetAngle);
			Debug.Log ("SetAngle:" + angle_str);
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
		if (Horn.localRotation.z > (Mathf.PI / 180) * targetAngle)
		{
			Horn.transform.Rotate(Vector3.back * Time.deltaTime * Speed);
		}
		else
		{
			Horn.transform.Rotate(Vector3.forward * Time.deltaTime * Speed);
		}
	}
	
	void OnPress (bool isPressed) 
	{
		Debug.Log ("Servy OnPress");
		PropertyManager.Instance.ShowProperty (Guid);
	}

}
