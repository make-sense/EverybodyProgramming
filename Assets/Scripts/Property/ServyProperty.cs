using UnityEngine;
using System.Collections;

public class ServyProperty : MonoBehaviour {

	public void OnAngleLeft()
	{
		Actor actor = ActorManager.Instance.Get (PropertyManager.Instance.currentGuid);
		if (actor == null)
			return;
		((Servy)actor).SetAngle ("180");
	}
	
	public void OnAngleMiddleLeft()
	{
		Actor actor = ActorManager.Instance.Get (PropertyManager.Instance.currentGuid);
		if (actor == null)
			return;
		((Servy)actor).SetAngle ("135");
	}
	
	public void OnAngleMiddle()
	{
		Actor actor = ActorManager.Instance.Get (PropertyManager.Instance.currentGuid);
		if (actor == null)
			return;
		((Servy)actor).SetAngle ("90");
	}
	
	public void OnAngleMiddleRight()
	{
		Actor actor = ActorManager.Instance.Get (PropertyManager.Instance.currentGuid);
		if (actor == null)
			return;
		((Servy)actor).SetAngle ("45");
	}
	
	public void OnAngleRight()
	{
		Actor actor = ActorManager.Instance.Get (PropertyManager.Instance.currentGuid);
		if (actor == null)
			return;
		((Servy)actor).SetAngle ("\t0");
	}

	private static ServyProperty _instance = null;
	public static ServyProperty Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(ServyProperty)) as ServyProperty;
			}
			return _instance;
		}
	}
}
