using UnityEngine;
using System.Collections;

public class Sandy : Actor {

	public void WaitMiliSecond()
	{
//		int sleep_ms = System.Convert.ToInt32 (milisec);
//		Debug.Log ("Sandy:WaitMiliSecond => " + milisec.ToString ());
		System.Threading.Thread.Sleep (1000);
//		Debug.Log ("[End]Sandy:WaitMiliSecond => " + milisec.ToString ());
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
