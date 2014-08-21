/* Butty.cs
 * Butty is button character
 * It can be act like button and can be attached arduino pin
 * Default pin is 8
 */

using UnityEngine;
using System.Collections;
using Uniduino;

public class Butty : Actor {

	Arduino arduino;
	private bool _configured = false;

	public enum STATE 
	{
		NONE,
		RELEASING,
		TOUCHDOWN,
		TOUCHING,
		TOUCHUP,
	};
	private STATE _state = STATE.RELEASING;
	public STATE CheckState 
	{
		get 
		{
			return _state;
		}
	}

	public bool IsTouchDown ()
	{
//		Debug.Log ("Butty:State : " + CheckState);
		if (CheckState == STATE.TOUCHDOWN)
			return true;
		return false;
	}
	
	public bool IsTouching ()
	{
//		Debug.Log ("Butty:State : " + CheckState);
		if (CheckState == STATE.TOUCHING)
			return true;
		return false;
	}
	
	public bool IsTouchUp ()
	{
//		Debug.Log ("Butty:State : " + CheckState);
		if (CheckState == STATE.TOUCHUP)
			return true;
		return false;
	}
	
	public bool IsReleasing ()
	{
//		Debug.Log ("Butty:State : " + CheckState);
		if (CheckState == STATE.RELEASING)
			return true;
		return false;
	}
	
	public int pin = -1;
	bool swButtonPressed = false;

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
		arduino.pinMode(pin, PinMode.INPUT);
		arduino.digitalWrite(pin, Arduino.HIGH);
		arduino.reportDigital((byte)(pin/8), 1);
		_configured = true;
	}

	public void Start () {
		base.Start ();
		ActorManager.Instance.Add (this);
		arduino = Arduino.global;
	}

	public override void Refresh () {
//		Debug.Log ("Butty::Refresh");
		if (_configured) 
		{
			int value = arduino.digitalRead (pin);
			ChangeState ((value == Arduino.LOW) ? true : false);	// pressing button is LOW
//			Debug.Log (value.ToString());
		} 
		else 
		{
			ChangeState (swButtonPressed);
		}
	}

	// Update is called once per frame
	void Update () {
	}

	void OnPress (bool isPressed) 
	{
		swButtonPressed = isPressed;
//		ChangeState (swButtonPressed);
//		Debug.Log ("Butty OnPress with " + isPressed.ToString ());
		PropertyManager.Instance.ShowButtyProperty (Guid);
	}

	private void ChangeSprite()
	{
		switch(_state)
		{
			case STATE.TOUCHDOWN:
			case STATE.TOUCHING:
			{
				UISprite sprite = GetComponentInChildren<UISprite> () as UISprite;
				sprite.spriteName = "Button_Red";
				break;
			}
			default:
			{
				UISprite sprite = GetComponentInChildren<UISprite> () as UISprite;
				sprite.spriteName = "Button";
				break;
			}
		}
	}

	private void ChangeState(bool pushed)
	{
		switch (_state) 
		{
			case STATE.RELEASING:
			{
				if (!pushed)
					_state = STATE.RELEASING;
				else
					_state = STATE.TOUCHDOWN;
				break;
			}
			case STATE.TOUCHDOWN:
			{
				if (!pushed)
					_state = STATE.TOUCHUP;
				else
					_state = STATE.TOUCHING;
				break;
			}
			case STATE.TOUCHING:
			{
				if (!pushed)
					_state = STATE.TOUCHUP;
				else
					_state = STATE.TOUCHING;
				break;
			}
			case STATE.TOUCHUP:
			{
				if (!pushed)
					_state = STATE.RELEASING;
				else
					_state = STATE.TOUCHDOWN;
				break;
			}
			default:
			{
				if (!pushed)
					_state = STATE.RELEASING;
				else
					_state = STATE.TOUCHDOWN;
				break;
			}
		}
//		Debug.Log (_state.ToString());
		ChangeSprite ();
	}
}
