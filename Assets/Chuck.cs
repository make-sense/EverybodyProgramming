using UnityEngine;
using System.Collections;

public class Chuck : MonoBehaviour {

	public UIRoot uiRoot = null;
	private int CHUCK_WIDTH = 100;
	private int CHUCK_HEIGHT = 75;

	// Use this for initialization
	void Start () {
		Debug.Log("Start");
	}
	
	// Update is called once per frame
	void Update () {
//		if (this.transform.parent != uiRoot)
//			this.transform.localScale = new Vector3((float)CHUCK_WIDTH, 0f);
	}

	void OnCollisionEnter () {
		Debug.Log ("OnCollisionEnter");
	}

	void OnCollisionExit() {
		Debug.Log ("OnTriggerExit");
		this.transform.parent = uiRoot.transform;
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("OnTriggerEnter");
		if (other.tag == "Chuck") {
			if (isRightEdge(this.transform, other.transform))
			{
				this.transform.parent = other.transform;
				this.transform.localPosition = new Vector3((float)CHUCK_WIDTH, 0f);

				Debug.Log ("Set this to child");
			}
		}
	}

//	void OnTriggerStay(Collider other) {
//		Debug.Log ("OnTriggerStay");
//	}

	void OnTriggerExit(Collider other) {
		Debug.Log ("OnTriggerExit");
		if (other.tag == "Chuck") {
			if (uiRoot != null)
				this.transform.parent = uiRoot.transform;
		}
	}

	private bool isRightEdge(Transform src, Transform dst)
	{
		if (src.localPosition.x > dst.localPosition.x + CHUCK_WIDTH/2)
			return true;
		return false;
	}
}
