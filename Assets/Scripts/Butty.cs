/* Butty.cs
 * Butty is button character
 * It can be act like button and can be attached arduino pin
 * Default pin is 8
 */

using UnityEngine;
using System.Collections;
using Uniduino;

public class Butty : Actor {

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
	private STATE _state = STATE.NONE;
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

	public int pin = -1;
	private bool swButtonPressed = false;

	public void AttachPin(int p)
	{
		pin = p;
		if (pin >= 0)
		{
			Arduino.global.pinMode(pin, PinMode.INPUT);
			Arduino.global.digitalWrite(pin, Arduino.HIGH);
			UILabel uiLabel = this.GetComponentInChildren<UILabel>() as UILabel;
			uiLabel.text = "D" + pin.ToString();
		}
	}

	// Update is called once per frame
	void Update () {
		if (pin >= 0) 
		{
			if (!swButtonPressed)
			{
				if (Arduino.global.Connected)
				{
					int value = Arduino.global.digitalRead (pin);
					Debug.Log (value.ToString());
					ChangeState ((value == Arduino.LOW) ? true : false);	// pressing button is LOW
				}
			}
		}
	}

	void OnPress (bool isPressed) 
	{
		swButtonPressed = isPressed;
		ChangeState (swButtonPressed);
		Debug.Log ("Butty OnPress");
		PropertyManager.Instance.ShowButtyProperty (Guid);
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
		Debug.Log (_state.ToString());
	}
}
