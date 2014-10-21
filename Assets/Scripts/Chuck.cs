using UnityEngine;
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

	Color inputColor = new Color (217f/255f, 83f/255f, 79f/255f);
	Color outputColor = new Color (92f/255f, 184f/255f, 92f/255f);
	Color normalColor = new Color (240f/255f, 173f/255f, 78f/255f);

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

	public void SetAction(System.Guid actorID, int actionID)
	{
		actorGuid = actorID;
		actionGuid = actionID;
		SetActionUI();
	}

	public void SetAction(System.Guid actorID, int actionID, string param)
	{
		_param = param;
		SetAction (actorID, actionID);
	}

	private void UpdateChuckUI ()
	{
		Transform detail = this.transform.FindChild("Detail");
		UIButton baseButton = GetComponentInChildren<UIButton> () as UIButton;
		if (detail == null || !detail.gameObject.activeSelf)
		{
			if (_children[0] == null && _children[1] == null)
				baseButton.normalSprite = "chuck_arrow_base";
			else if (_children[0] != null && _children[1] == null)
				baseButton.normalSprite = "chuck_arrow_bottom";
			else if (_children[0] == null && _children[1] != null)
				baseButton.normalSprite = "chuck_arrow_right";
			else
				baseButton.normalSprite = "chuck_arrow_full";
		}
		else 
		{
			if (_children[0] == null && _children[1] == null)
				baseButton.normalSprite = "chuck_base";
			else if (_children[0] != null && _children[1] == null)
				baseButton.normalSprite = "chuck_bottom";	
			else if (_children[0] == null && _children[1] != null)
				baseButton.normalSprite = "chuck_right";
			else
				baseButton.normalSprite = "chuck_full";
		}
	}

	private static void UpdateChuckUIAll ()
	{
		BetterList<Chuck> chucks = ChuckManager.Instance.GetChucks ();
		foreach (Chuck chuck in chucks) 
		{
			chuck.UpdateChuckUI ();
		}
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
				UISprite sprite = button.GetComponentInChildren<UISprite> () as UISprite;
				sprite.MakePixelPerfect ();
			}
			UpdateChuckUI ();
		}
	}

	void OnDoubleClick () {
		Debug.Log ("Chuck OnDoubleClick:"+Guid.ToString ());
		Execute ();
	}

	public void Execute () {
		if (_status != eChuckStatus.READY)
			return;

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
					if (actionData.IsParamNeed)
						actor.gameObject.BroadcastMessage (actionData.CallFunctionName, _param);
					else
						actor.gameObject.BroadcastMessage (actionData.CallFunctionName, actionData.CallFunctionParam);

					if (actionData.CallFunctionName=="WaitSecond")
					{
						TimeLength_ms = (int)(System.Convert.ToDouble (_param) * 1000);
						Debug.Log ("Set chuck's execute time to " + TimeLength_ms + "ms");
					}

					DateTime begin = DateTime.Now;
					while (true)
					{
						TimeSpan timeSpan = DateTime.Now.Subtract (begin);
						if (timeSpan.TotalMilliseconds > TimeLength_ms)
							break;
						else
							yield return new WaitForSeconds (0.01f);
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

		checkDone ();

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

	void checkDone ()
	{
		if (_children [0] == null && _children [1] == null)
			_status = eChuckStatus.READY;
		else if (_children [0] != null && _children [0]._status == eChuckStatus.READY)
			_status = eChuckStatus.READY;
		else if (_children [1] != null && _children [1]._status == eChuckStatus.READY)
			_status = eChuckStatus.READY;

		if (_status == eChuckStatus.READY) 
		{
			if (!IsRoot())
			{
				Chuck parent = transform.parent.GetComponentInChildren<Chuck> () as Chuck;
				parent.checkDone ();
			}
		}
	}

	// Use this for initialization
	void Start () {
		Guid = System.Guid.NewGuid ();
		ChuckManager.Instance.Add(this);
		if (UIRoot.list.Count > 0)
			_uiRoot = UIRoot.list[0];
	}
	
	void OnPress (bool isPressed) 
	{
//		Debug.Log ("Chuck OnPress:"+Guid.ToString ());
		ChuckPropertyManager.Instance.Show ();
		ChuckPropertyManager.SelectedChuckGuid = Guid;

		UpdateChuckUI ();
		if (!isPressed)
		{
			if (isChuckSeparated (this.transform))
			{
				this.transform.parent = _uiRoot.transform;
				UpdateChuckUIAll ();
			}
		}
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
			UpdateChuckUIAll ();
		}
		else if (other.tag == "ChuckStack")
		{
			DestroyRecursively (this);
		}
	}

	void DestroyRecursively (Chuck chuck)
	{
		if (chuck._children [0] != null)
			DestroyRecursively (chuck._children [0]);
		if (chuck._children [1] != null)
			DestroyRecursively (chuck._children [1]);
//		Debug.Log ("Destroy Chuck: " + chuck.Guid.ToString ());
		ChuckManager.Instance.Remove (chuck);
		Destroy (chuck.gameObject);
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
		if ((src.transform.localPosition.x < CHUCK_WIDTH - CHUCK_WIDTH*0.2f || 
			src.transform.localPosition.x > CHUCK_WIDTH + CHUCK_WIDTH*0.2f) &&
		    (src.transform.localPosition.y < -CHUCK_HEIGHT - CHUCK_HEIGHT*0.2f ||
		    src.transform.localPosition.y > -CHUCK_HEIGHT + CHUCK_HEIGHT*0.2f))
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
