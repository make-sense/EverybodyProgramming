using UnityEngine;
using System;
using System.Collections;
using System.Threading;

public class Sandy : Actor {

	private int requested_ms;
	public void WaitSecond(string time_sec)
	{
		//		double sleep_sec = System.Convert.ToDouble (time_sec);
//		int sleep_ms = (int)(sleep_sec * 1000);
//		requested_ms = sleep_ms;
////		Debug.Log ("Sandy:WaitMiliSecond => " + sleep_ms.ToString ());
////		Thread.Sleep (sleep_ms);
//		Thread thread = new Thread (new ThreadStart (Delay));
//		thread.Start ();
//		while (thread.IsAlive)
//						;
////		StartCoroutine ("Delay", sleep_sec * 1000);
//		Debug.Log ("[End]Sandy:WaitMiliSecond => " + sleep_ms.ToString ());
	}

	void Delay ()
	{
		DateTime begin = DateTime.Now;
		while (true)
		{
			TimeSpan timeSpan = DateTime.Now.Subtract (begin);
			if (timeSpan.TotalMilliseconds > requested_ms)
				break;
		}
	}

//	IEnumerator Delay (double delay_ms)
//	{
//		DateTime begin = DateTime.Now;
//		while (true)
//		{
//			TimeSpan timeSpan = DateTime.Now.Subtract (begin);
//			if (timeSpan.TotalMilliseconds > delay_ms)
//				break;
//			else
//				yield return new WaitForSeconds (0.01f);
//		}
//	}

	public override void Refresh ()
	{
	}

	// Use this for initialization
	public void Start () {
		base.Start ();
		ActorManager.Instance.Add (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
