using UnityEngine;
using System.Collections;

public class Sandy : Actor {

	public void WaitMiliSecond(string milisec)
	{
		int sleep_ms = System.Convert.ToInt32 (milisec);
		System.Threading.Thread.Sleep (sleep_ms);
	}

	// Use this for initialization
	public void Start () {
		base.Start ();
		base.charactorType = Actor.eCharactor.SANDY;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
