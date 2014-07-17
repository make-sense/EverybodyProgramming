﻿using UnityEngine;
using System.Collections;

public class Chuck : MonoBehaviour {

	private float CHUCK_HEIGHT = 75f;
	private float CHUCK_WIDTH = 100f;

	private UIRoot _uiRoot = null;
	private Chuck[] _children = new Chuck[2];

	// Use this for initialization
	void Start () {
		if (UIRoot.list.Count > 0)
			_uiRoot = UIRoot.list[0];
//		Debug.Log("Start");
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider other) 
	{
//		Debug.Log ("OnTriggerEnter");
		if (other.tag == "Chuck") 
		{
			Vector3 srcPos = getGlobalPosition(this.transform);
			Vector3 dstPos = getGlobalPosition(other.transform);
//			Debug.Log (srcPos.ToString() + "<=>" + dstPos.ToString());
			if (isRightEdge(srcPos, dstPos))
			{
				this.transform.parent = other.transform;
				this.transform.localPosition = new Vector3(CHUCK_WIDTH, 0);
//				Debug.Log ("Set this to right child");
			}
			else if (isBottomEdge(srcPos, dstPos))
	        {
				this.transform.parent = other.transform;
				this.transform.localPosition = new Vector3(0, -CHUCK_HEIGHT);
//				Debug.Log ("Set this to bottom child");
			}
		}
		else if (other.tag == "ChuckStack")
		{
			Debug.Log ("Destroy " + gameObject.name);
			Destroy(this.gameObject);
		}
	}

	void OnDragEnd () 
	{
		if (isRoot(this.transform))
		    return;

		if (isChuckSeparated(this.transform))
		{
//			Debug.Log ("isChuckDisconnect");
			this.transform.parent = _uiRoot.transform;
		}
//		Debug.Log ("OnDragEnd");
	}

	void OnDragDropRelease(GameObject surface)
	{
		Debug.Log ("OnDragDropRelease");
	}

//	void OnTriggerExit(Collider other) 
//	{
//		Debug.Log ("OnTriggerExit");
//		if (other.tag == "Chuck")
//		{
//			if (UIRoot.list.Count > 0)
//				this.transform.parent = UIRoot.list[0].transform;
//		}
//	}

	private bool isRightEdge(Vector3 src, Vector3 dst) 
	{
//		Debug.Log ("isRightEdge");
		if (src.x > dst.x + CHUCK_WIDTH/2 &&
		    src.x < dst.x + CHUCK_WIDTH &&
		    src.y > dst.y - CHUCK_HEIGHT/2 &&
		    src.y < dst.y + CHUCK_HEIGHT)
			return true;
		return false;
	}

	private bool isBottomEdge(Vector3 src, Vector3 dst)
	{
//		Debug.Log ("isBottomEdge");
		if (src.x > dst.x &&
		    src.x < dst.x + CHUCK_WIDTH/2 &&
		    src.y > dst.y - CHUCK_HEIGHT &&
		    src.y < dst.y - CHUCK_HEIGHT/2)
			return true;
		return false;
	}

	private bool isChuckSeparated(Transform src)
	{
		if (src.transform.localPosition.x < CHUCK_WIDTH -CHUCK_WIDTH*0.2f || 
			src.transform.localPosition.x > CHUCK_WIDTH + CHUCK_WIDTH*0.2f ||
		    src.transform.localPosition.y < -CHUCK_HEIGHT - CHUCK_HEIGHT*0.2f ||
		    src.transform.localPosition.y > -CHUCK_HEIGHT + CHUCK_HEIGHT*0.2f)
			return true;
		return false;
	}

	private Vector3 getGlobalPosition(Transform transform)
	{
		if (isRoot (transform))
			return transform.localPosition;
		else
		{
			Vector3 pos = getGlobalPosition(transform.parent);
			if (isRightChild(transform))
				return new Vector3(pos.x+CHUCK_WIDTH, pos.y, 0);
			else
				return new Vector3(pos.x, pos.y-CHUCK_HEIGHT, 0);
		}
	}

	private bool isRoot(Transform transform)
	{
		if (transform.parent == null)
			return true;

		if (!transform.parent.name.Contains("Chuck"))
			return true;
		return false;
	}

	private bool isRightChild(Transform transform)
	{
		if (isRoot (transform))
			return false;
		if (transform.localPosition.x > CHUCK_WIDTH/2)
			return true;
		return false;
	}

	private bool isBottomChild(Transform transfor)
	{
		if (isRoot (transform))
			return false;
		if (transform.localPosition.y < -CHUCK_HEIGHT/2)
			return true;
		return false;
	}
}
