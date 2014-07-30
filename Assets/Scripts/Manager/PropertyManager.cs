using UnityEngine;
using System.Collections;

public class PropertyManager : MonoBehaviour {

	public GameObject ActorName;
	public GameObject propertyRoot;
	public GameObject propertyDetail;

	Actor.eCharactor selectedCharactor = Actor.eCharactor.NONE;
	System.Guid currentGuid;

	// Use this for initialization
	void Start () {
		propertyRoot = GameObject.Find ("Property");
		if (propertyRoot != null)
			Hide ();
		else
			Debug.Log ("Can't find Property GameObject");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowButtyProperty (System.Guid guid) {
		currentGuid = guid;
		Show ();
		selectedCharactor = Actor.eCharactor.BUTTY;
		Actor actor = ActorManager.Instance.Get (currentGuid);
		if (actor != null)
		{
			UILabel label = ActorName.GetComponentInChildren<UILabel> () as UILabel;
			if (label != null)
			{
				label.text = actor.ActorName;
			}
			else
			{
				Debug.Log ("Fail to found UIInput");
			}
			Debug.Log ("Found actor");
		}
		else
		{
			Debug.Log ("Fail to found");
		}
	}

	public void Show () {
		propertyRoot.SetActive(true);
	}

	public void Hide () {
		propertyRoot.SetActive(false);
	}

	public void Set () {
		UILabel label = ActorName.GetComponentInChildren<UILabel> () as UILabel;
		Actor actor = ActorManager.Instance.Get (currentGuid);
		actor.ActorName = label.text;
		switch (selectedCharactor) 
		{
			case Actor.eCharactor.BUTTY:
			{
				GameObject gameObject = GameObject.Find ("ButtyProperty");
				if (gameObject != null)
				{
					UIPopupList list = gameObject.GetComponentInChildren<UIPopupList> () as UIPopupList;
					Debug.Log (list.value.Substring(1));
					((Butty)actor).AttachPin(System.Convert.ToInt32(list.value.Substring(1)));
				}
				break;
			}
		}
	}

	private static PropertyManager _instance = null;
	public static PropertyManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(PropertyManager)) as PropertyManager;
			}
			return _instance;
		}
	}
}
