/* Bulby.cs
 * Bulby is bulb character
 * It can be act like LED and can be attached arduino pin
 * Red:D5, Green:D6, Blue:D9
 */

using UnityEngine;
using System.Collections;
using Uniduino;

public class Bulby : Actor {

	Arduino arduino;
	private bool _configured = false;
	public int pinR = -1;
	public int pinG = -1;
	public int pinB = -1;

	public void AttachPins(int red, int green = -1, int blue = -1)
	{
		pinR = red;
		pinG = green;
		pinB = blue;
		if (pinR >= 0 || pinG >= 0 || pinB >= 0)
		{
			_configured = false;
			arduino.Setup(ConfigurePin);
		}
	}
	
	void ConfigurePin ()
	{
		if (pinR >= 0)
			arduino.pinMode(pinR, PinMode.OUTPUT);
		if (pinG >= 0)
			arduino.pinMode(pinG, PinMode.OUTPUT);
		if (pinB >= 0)
			arduino.pinMode(pinB, PinMode.OUTPUT);
		_configured = true;
	}

	public void SetColor(Color color)
	{
		if (!_configured)
			return;

		UISprite sprite = GetComponent<UISprite> () as UISprite;
		sprite.color = color;

		if (pinR >= 0)
			arduino.digitalWrite (pinR, (int)(color.r * 255));
		if (pinG >= 0)
			arduino.digitalWrite (pinG, (int)(color.g * 255));
		if (pinB >= 0)
			arduino.digitalWrite (pinB, (int)(color.b * 255));
	}

	// Use this for initialization
	void Start () {
		base.Start ();
		arduino = Arduino.global;
	}
	

	// Update is called once per frame
	void Update () {
	
	}

	void OnPress (bool isPressed) 
	{
		Debug.Log ("Bulby OnPress");
		PropertyManager.Instance.ShowBulbyProperty (Guid);
	}

	private void ChangeSplite()
	{
	}
}
