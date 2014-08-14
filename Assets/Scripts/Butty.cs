﻿/* Butty.cs
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
		TOUCHDOWN_CHECKED,
		TOUCHING,
		TOUCHUP,
		TOUCHUP_CHECKED,
	};
	private STATE _state = STATE.RELEASING;
	public STATE CheckState 
	{
		get 
		{
			switch(_state)
			{
			case STATE.TOUCHDOWN:
				_state = STATE.TOUCHDOWN_CHECKED;
				return STATE.TOUCHDOWN;
			case STATE.TOUCHUP:
				_state = STATE.TOUCHUP_CHECKED;
				return STATE.TOUCHUP;
			default:
				return _state;
			}
		}
	}

	public void IsTouchDown(out bool resultValue)
	{
		Debug.Log ("Butty:State : " + CheckState);
		if (CheckState == STATE.TOUCHDOWN)
			resultValue = true;
		resultValue = false;
	}
	
	public bool IsTouching()
	{
		Debug.Log ("Butty:State : " + CheckState);
		if (CheckState == STATE.TOUCHING)
			return true;
		return false;
	}
	
	public void IsTouchUp(out bool resultValue)
	{
		Debug.Log ("Butty:State : " + CheckState);
		if (CheckState == STATE.TOUCHUP)
			resultValue = true;
		resultValue = false;
	}
	
	public void IsReleasing(out bool resultValue)
	{
		Debug.Log ("Butty:State : " + CheckState);
		if (CheckState == STATE.RELEASING)
			resultValue = true;
		resultValue = false;
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
		base.charactorType = eCharactor.BUTTY;
		arduino = Arduino.global;
	}

	// Update is called once per frame
	void Update () {
		if (pin >= 0) 
		{
			if (!swButtonPressed)
			{
				if (_configured)
				{
					int value = arduino.digitalRead (pin);
					ChangeState ((value == Arduino.LOW) ? true : false);	// pressing button is LOW
//					Debug.Log (value.ToString());
				}
			}
		}
	}

	void OnPress (bool isPressed) 
	{
		swButtonPressed = isPressed;
		ChangeState (swButtonPressed);
//		Debug.Log ("Butty OnPress");
		PropertyManager.Instance.ShowButtyProperty (Guid);
	}

	private void ChangeSprite()
	{
		switch(_state)
		{
			case STATE.TOUCHDOWN:
			case STATE.TOUCHDOWN_CHECKED:
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
			case STATE.TOUCHDOWN_CHECKED:
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
			case STATE.TOUCHUP_CHECKED:
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
