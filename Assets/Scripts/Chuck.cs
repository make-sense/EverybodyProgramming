using UnityEngine;
using System.Collections;

public class Chuck : MonoBehaviour {

	private float CHUCK_HEIGHT = 110f;
	private float CHUCK_WIDTH = 152f;

	public System.Guid Guid;
	private UIRoot _uiRoot = null;
	private Chuck[] _children = new Chuck[2];

	public System.Guid actorGuid;
	public int actionGuid;

	Color inputColor = new Color(1f, 0.5f, 0.5f);
	Color outputColor = new Color(0.5f, 0.5f, 1f);
	
	public void SetAction(System.Guid actorID, int actionID)
	{
		actorGuid = actorID;
		actionGuid = actionID;
		SetActionUI();
	}

	private void SetActionUI() {
		Actor actor = ActorManager.Instance.Get (actorGuid);
		if (actor != null) {
			UIButtonColor baseButtonColor = GetComponentInChildren<UIButtonColor> () as UIButtonColor;
			UIButton baseButton = GetComponentInChildren<UIButton> () as UIButton;
			baseButton.normalSprite = "chuck_base";
			Transform detail = this.transform.FindChild("Detail");
			if (detail != null) {
				detail.gameObject.SetActive(true);
				UIButton button = detail.GetComponentInChildren<UIButton> () as UIButton;
				switch (actor.charactorType)
				{
					case Actor.eCharactor.BUTTY:
					{
						baseButtonColor.defaultColor = inputColor;
						switch (actionGuid) 
						{
							case -63941309:
								button.normalSprite = "1407589060_Perspective Button - Games";
								break;
							case -1620462626:
								button.normalSprite = "Button_Red";
								break;
							case -1483390853:
								button.normalSprite = "1407862950_Perspective Button - Favorites";
								break;
						}
						break;
					}
					case Actor.eCharactor.BULBY:
					{
						baseButtonColor.defaultColor = outputColor;
						switch (actionGuid)
						{
							case -526438998:
								button.normalSprite = "1407405112_Black_button";
								break;
							case -844790351:
								button.normalSprite = "1407922654_Silver_button";
								break;
							case -323650059:
								button.normalSprite = "1407405125_Red_button";
								break;
							case -268715499:
								button.normalSprite = "1407405122_Green_button";
								break;
							case -1310113180:
								button.normalSprite = "1407405123_Dark_blue_button";
								break;
							case -135481751:
								button.normalSprite = "1407405117_Yellow_button";
								break;
							case -356674938:
								button.normalSprite = "1407405119_Orange_button";
								break;
							case -1155006195:
								button.normalSprite = "1407405121_Light_blue_button";
								break;
						}
						break;
					}
				}
			}
		}
	}

	void OnDoubleClick () {
		Debug.Log ("Chuck OnDoubleClick:"+Guid.ToString ());
		Execute ();
	}

	void Execute () {
		// 1. check state

		// 2. run this
		ActionData actionData = ActionManager.Instance.GetActionData(actionGuid);
		if (actionData != null) 
		{
			Actor actor = ActorManager.Instance.Get(actorGuid);
			if (actor != null)
			{
				actor.gameObject.BroadcastMessage(actionData.CallFunctionName);
				Debug.Log ("Action:" + actionData.CallFunctionName);
			}
		}

		// 3. run bottom chuck

		// 4. if end this, run right chuck
	}

	// Use this for initialization
	void Start () {
		Guid = System.Guid.NewGuid ();
		ChuckManager.Instance.Add(this);
		if (UIRoot.list.Count > 0)
			_uiRoot = UIRoot.list[0];
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
			ChuckManager.Instance.Remove(this);
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
