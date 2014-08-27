﻿using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using System.Threading;

public class Chuck : MonoBehaviour {

	private float CHUCK_WIDTH = 90f;
	private float CHUCK_HEIGHT = 120f;

	public System.Guid Guid;
	private UIRoot _uiRoot = null;
	public Chuck[] _children = new Chuck[2];

	public System.Guid actorGuid;
	public int actionGuid;
	string _param;

	int TimeLength_ms = 100;

	Color inputColor = new Color (1f, 0.5f, 0.5f);
	Color outputColor = new Color (0f, 200/255f, 100/255f);
	Color normalColor = new Color (230f/255f, 180f/255f, 30f/255f);

	public enum eChuckStatus {
		NONE,
		READY,
		RUNNING,
		DONE,
		WARNING,
		ERROR,
	};
	eChuckStatus _status = eChuckStatus.READY;
	public eChuckStatus Status {
		get {
			return _status;
		}
	}

	public UIButton startButton;

	public bool _isStart = false;

	public void OnToggleStart ()
	{
		_isStart = !_isStart;
		UpdateStartIcon();
	}

	private void UpdateStartIcon()
	{
		if (_isStart) 
		{
			startButton.normalSprite = "1408105883_traffic_lights_green";
		}
		else
		{
			startButton.normalSprite = "1408105883_traffic_lights_red";
		}
	}

	public void SetAction(System.Guid actorID, int actionID)
	{
		actorGuid = actorID;
		actionGuid = actionID;
		SetActionUI();
	}

	public void SetAction(System.Guid actorID, int actionID, string param)
	{
		actorGuid = actorID;
		actionGuid = actionID;
		_param = param;
		SetActionUI();
	}

	private void SetActionUI() {
		Actor actor = ActorManager.Instance.Get (actorGuid);
		if (actor != null) {
			UIButtonColor baseButtonColor = GetComponentInChildren<UIButtonColor> () as UIButtonColor;
			ActionData actionData = ActionManager.Instance.GetActionData(actionGuid);
			if (actionData != null) {
				if (actionData.Type == eActionType.Input)
					baseButtonColor.defaultColor = inputColor;
				else
					baseButtonColor.defaultColor = outputColor;
			}
			else {
				baseButtonColor.defaultColor = normalColor;
			}

			UIButton baseButton = GetComponentInChildren<UIButton> () as UIButton;
			baseButton.normalSprite = "chuck_base";
			Transform detail = this.transform.FindChild("Detail");
			if (detail != null) {
				detail.gameObject.SetActive(true);
				UIButton button = detail.GetComponentInChildren<UIButton> () as UIButton;
				button.normalSprite = actionData.texture.name;
			}
		}
	}

	void OnDoubleClick () {
		Debug.Log ("Chuck OnDoubleClick:"+Guid.ToString ());
		Execute ();
	}

	public void Execute () {
		// 1. check state
//		Debug.Log ("Execute = " + Guid.ToString ());
		ActionData actionData = ActionManager.Instance.GetActionData(actionGuid);
		if (actionData != null) 
		{
			Actor actor = ActorManager.Instance.Get(actorGuid);
			if (actor != null)
			{
				if (actionData.Type == eActionType.Input) {
					MethodInfo methodInfo = actor.GetType().GetMethod(actionData.CallFunctionName);
					object result;
					if (actionData.IsParamNeed) {
						object[] parameters = new object[] { _param };
						result = methodInfo.Invoke(actor, parameters);
					}
					else {
						result = methodInfo.Invoke(actor, null);
					}

					if (result.ToString() == "False")
						return;
					else
						Debug.Log ("Action:" + actionData.CallFunctionName + "=>" + result.ToString ());
				}
			}
		}

		StartCoroutine ("Execute_Co");
		StartCoroutine ("Execute_Bottom");
	}

	IEnumerator Execute_Co ()
	{
		Debug.Log ("Execute Co:" + Guid.ToString ());

		// 2. run this
		_status = eChuckStatus.RUNNING;

		ActionData actionData = ActionManager.Instance.GetActionData(actionGuid);
		if (actionData != null) 
		{
			Actor actor = ActorManager.Instance.Get (actorGuid);
			if (actor != null)
			{
				if (actionData.Type == eActionType.Output) 
				{
					actor.gameObject.BroadcastMessage (actionData.CallFunctionName, actionData.CallFunctionParam);
					DateTime begin = DateTime.Now;
					while (true) 
					{
						TimeSpan timeSpan = DateTime.Now.Subtract (begin);
						if (timeSpan.TotalMilliseconds > TimeLength_ms)
							break;
						else
							yield return new WaitForSeconds (0.1f);
					}
					Debug.Log ("Action:" + actionData.CallFunctionName);
				}
			}
		}
		else
		{
			_status = eChuckStatus.ERROR;
		}

		if (_children [0] != null) {
			while (_children [0].Status == eChuckStatus.READY ||
					_children [0].Status == eChuckStatus.RUNNING)
				yield return new WaitForSeconds (0.1f);
		}

		_status = eChuckStatus.DONE;
		
		// 4. if end this, run right chuck
		if (_children [1] != null) 
		{
			Debug.Log ("Execute Right:" + Guid.ToString ());
			_children [1].Execute ();
		}

		yield return null;
	}

	IEnumerator Execute_Bottom ()
	{
		// 3. run bottom chuck
		if (_children [0] != null) {
			//			Thread thread = new Thread (new ThreadStart (_children [0].Execute));
			//			thread.Start ();
			Debug.Log ("Execute Bottom:" + Guid.ToString ());
			_children [0].Execute ();
		}
		yield return null;
	}

	// Use this for initialization
	void Start () {
		Guid = System.Guid.NewGuid ();
		ChuckManager.Instance.Add(this);
		if (UIRoot.list.Count > 0)
			_uiRoot = UIRoot.list[0];
		UpdateStartIcon ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnPress (bool isPressed) 
	{
		Debug.Log ("Chuck OnPress:"+Guid.ToString ());
		ChuckPropertyManager.Instance.Show ();
		ChuckPropertyManager.SelectedChuckGuid = Guid;
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Chuck") 
		{
			Vector3 srcPos = getGlobalPosition(this.transform);
			Vector3 dstPos = getGlobalPosition(other.transform);
			if (isRightEdge(srcPos, dstPos))
			{
				this.transform.parent = other.transform;
				this.transform.localPosition = new Vector3(CHUCK_WIDTH, 0);
				Chuck rootChuck = other.GetComponentInChildren<Chuck> () as Chuck;
				if (rootChuck != null)
					rootChuck._children[1] = this;
			}
			else if (isBottomEdge(srcPos, dstPos))
	        {
				this.transform.parent = other.transform;
				this.transform.localPosition = new Vector3(0, -CHUCK_HEIGHT);
				Chuck rootChuck = other.GetComponentInChildren<Chuck> () as Chuck;
				if (rootChuck != null)
					rootChuck._children[0] = this;
			}
		}
		else if (other.tag == "ChuckStack")
		{
			Debug.Log ("Destroy " + gameObject.name);
			ChuckManager.Instance.Remove (this);
			Destroy(this.gameObject);
		}
	}

	void OnDragEnd () 
	{
		if (isRoot(this.transform))
		    return;

		if (isChuckSeparated(this.transform))
			this.transform.parent = _uiRoot.transform;
	}

	void OnDragDropRelease(GameObject surface)
	{
		Debug.Log ("OnDragDropRelease");
	}

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

	public bool IsRoot()
	{
		if (transform != null)
			return isRoot (transform);
		return false;
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
