using UnityEngine;
using System.Collections;

public class DcMotorProperty : MonoBehaviour {

	public void OnPowerOff()
	{
		Actor actor = ActorManager.Instance.Get (PropertyManager.Instance.currentGuid);
		if (actor == null)
			return;
		((DcMotor)actor).SetPower ("0");
	}
	
	public void OnPowerOn()
	{
		Actor actor = ActorManager.Instance.Get (PropertyManager.Instance.currentGuid);
		if (actor == null)
			return;
		((DcMotor)actor).SetPower ("255");
	}
	
	private static DcMotor _instance = null;
	public static DcMotor Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(DcMotor)) as DcMotor;
			}
			return _instance;
		}
	}
}
