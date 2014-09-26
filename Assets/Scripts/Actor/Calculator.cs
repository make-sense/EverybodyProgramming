using UnityEngine;
using System.Collections;

public class Calculator : Actor {

	int _value;

	public bool IsLess (string value)
	{
		int cmp = System.Convert.ToInt32 (value);
		if (_value < cmp)
			return true;
		return false;
	}
	
	public bool IsLessEqual (string value)
	{
		int cmp = System.Convert.ToInt32 (value);
		if (_value <= cmp)
			return true;
		return false;
	}
	
	public bool IsEqual (string value)
	{
		int cmp = System.Convert.ToInt32 (value);
		if (_value == cmp)
			return true;
		return false;
	}
	
	public bool IsLargerEqual (string value)
	{
		int cmp = System.Convert.ToInt32 (value);
		if (_value >= cmp)
			return true;
		return false;
	}
	
	public bool IsLarger (string value)
	{
		int cmp = System.Convert.ToInt32 (value);
		if (_value > cmp)
			return true;
		return false;
	}
	public int GetValue ()
	{
		return _value;
	}

	public void SetValue (int number)
	{
		_value = number;
	}

	public void Add (int number)
	{
		_value += number;
	}

	public void Sub (int number)
	{
		_value -= number;
	}

	public void Mul (int number)
	{
		_value *= number;
	}

	public void Div (int number)
	{
		_value /= number;
	}

	public void Start () 
	{
		base.Start ();
		ActorManager.Instance.Add (this);
	}
	
	public override void Refresh () 
	{
	}
	
	void OnPress (bool isPressed) 
	{
		PropertyManager.Instance.ShowProperty (Guid);
	}
}
