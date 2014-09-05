using UnityEngine;
using System.Collections;

public class BulbyProperty : MonoBehaviour {

	public void OnClickBlack()
	{
		Actor actor = ActorManager.Instance.Get (PropertyManager.Instance.currentGuid);
		if (actor == null)
			return;
		((Bulby)actor).SetColor (Color.black);
	}
	
	public void OnClickWhite()
	{
		Actor actor = ActorManager.Instance.Get (PropertyManager.Instance.currentGuid);
		if (actor == null)
			return;
		((Bulby)actor).SetColor (Color.white);
	}
	
	public void OnClickRed()
	{
		Actor actor = ActorManager.Instance.Get (PropertyManager.Instance.currentGuid);
		if (actor == null)
			return;
		((Bulby)actor).SetColor (Color.red);
	}
	
	public void OnClickGreen()
	{
		Actor actor = ActorManager.Instance.Get (PropertyManager.Instance.currentGuid);
		if (actor == null)
			return;
		((Bulby)actor).SetColor (Color.green);
	}
	
	public void OnClickBlue()
	{
		Actor actor = ActorManager.Instance.Get (PropertyManager.Instance.currentGuid);
		if (actor == null)
			return;
		((Bulby)actor).SetColor (Color.blue);
	}
	
	public void OnClickYellow()
	{
		Actor actor = ActorManager.Instance.Get (PropertyManager.Instance.currentGuid);
		if (actor == null)
			return;
		((Bulby)actor).SetColor (Color.yellow);
	}
	
	public void OnClickMagenta()
	{
		Actor actor = ActorManager.Instance.Get (PropertyManager.Instance.currentGuid);
		if (actor == null)
			return;
		((Bulby)actor).SetColor (Color.magenta);
	}
	
	public void OnClickCyan()
	{
		Actor actor = ActorManager.Instance.Get (PropertyManager.Instance.currentGuid);
		if (actor == null)
			return;
		((Bulby)actor).SetColor (Color.cyan);
	}
}
