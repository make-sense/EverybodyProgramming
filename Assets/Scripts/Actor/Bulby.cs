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

	public void SetOff () 
	{
		SetColor(Color.black);
	}
	public void SetOn ()
	{
		SetColor(Color.white);
	}
	public void SetRed ()
	{
		SetColor (Color.red);
	}
	public void SetGreen ()
	{
		SetColor (Color.green);
	}
	public void SetBlue ()
	{
		SetColor (Color.blue);
	}
	public void SetYellow ()
	{
		SetColor (Color.yellow);
	}
	public void SetMagenta ()
	{
		SetColor (Color.magenta);
	}
	public void SetCyan ()
	{
		SetColor (Color.cyan);
	}
	public void SetColorRed (string inValue)
	{
		int red = System.Convert.ToInt32 (inValue);
		SetColor (red, -1, -1);
	}
	public void SetColorGreen (string inValue)
	{
		int green = System.Convert.ToInt32 (inValue);
		SetColor (-1, green, -1);
	}
	public void SetColorBlue (string inValue)
	{
		int blue = System.Convert.ToInt32 (inValue);
		SetColor (-1, -1, blue);
	}
	Color[] colors = {Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan};
	public void SetRandom ()
	{
		int rand = Random.Range(0, 5);
		SetColor (colors[rand]);
	}

	public void SetColor(Color color)
	{
		UIButtonColor buttonColor = GetComponentInChildren<UIButtonColor> () as UIButtonColor;
		buttonColor.defaultColor = color;

		if (!_configured)
			return;
		
		if (pinR >= 0)
			arduino.digitalWrite (pinR, (int)(color.r * 255));
		if (pinG >= 0)
			arduino.digitalWrite (pinG, (int)(color.g * 255));
		if (pinB >= 0)
			arduino.digitalWrite (pinB, (int)(color.b * 255));
	}
	public void SetColor(int red, int green, int blue)
	{
		if (_configured)
		{
			if (red >= 0 && pinR >= 0)
				arduino.digitalWrite (pinR, red);
			if (green >= 0 && pinG >= 0)
				arduino.digitalWrite (pinG, green);
			if (blue >= 0 && pinB >= 0)
				arduino.digitalWrite (pinB, blue);
		}

		UIButtonColor buttonColor = GetComponentInChildren<UIButtonColor> () as UIButtonColor;

		if (red == -1)
			red = (int)(buttonColor.defaultColor.r * 255);
		if (green == -1)
			green = (int)(buttonColor.defaultColor.g * 255);
		if (blue == -1)
			blue = (int)(buttonColor.defaultColor.b * 255);

		buttonColor.defaultColor = new Color(((float)red)/255, ((float)green)/255, ((float)blue)/255);
	}

	// Use this for initialization
	public void Start () {
		base.Start ();
		ActorManager.Instance.Add (this);
		arduino = Arduino.global;
	}
	
	public override void Refresh () 
	{
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnPress (bool isPressed) 
	{
		Debug.Log ("Bulby OnPress");
		PropertyManager.Instance.ShowProperty (Guid);
	}

	private void ChangeSprite()
	{
	}
}
