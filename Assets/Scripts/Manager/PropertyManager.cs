using UnityEngine;
using System.Collections;

public class PropertyManager : MonoBehaviour {

	public GameObject ActorName;
	public GameObject propertyRoot;
	public GameObject propertyDetail;

	public System.Guid currentGuid;

	enum eCharactor {
		NONE,
		STAGE,
		BUTTY,
		CALCHY,
		BULBY,
		SANDY,
		SERVY,
	};
	eCharactor _selectedCharactor;

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

		foreach (Transform t in propertyDetail.transform) {
			if (t.name == "ButtyProperty")
				t.gameObject.SetActive(true);
			else
				t.gameObject.SetActive(false);
		}

		_selectedCharactor = eCharactor.BUTTY;
		Actor actor = ActorManager.Instance.Get (currentGuid);
		if (actor != null)
		{
			UILabel label = ActorName.GetComponentInChildren<UILabel> () as UILabel;
			if (label != null)
			{
				label.text = actor.ActorName;
			}
//			Debug.Log ("Found actor");
		}
		else
		{
			Debug.Log ("Fail to found");
		}
	}

	public void ShowBulbyProperty (System.Guid guid) {
		currentGuid = guid;
		Show ();

		foreach (Transform t in propertyDetail.transform) {
			if (t.name == "BulbyProperty")
				t.gameObject.SetActive(true);
			else
				t.gameObject.SetActive(false);
		}

		_selectedCharactor = eCharactor.BULBY;
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
				Debug.Log ("[ShowBulbyProperty]Fail to found UIInput");
			}
			Debug.Log ("[ShowBulbyProperty]Found actor");
		}
		else
		{
			Debug.Log ("[ShowBulbyProperty]Fail to found Actor : " + currentGuid.ToString ());
		}
	}

	public void ShowServyProperty (System.Guid guid) {
		currentGuid = guid;
		Show ();
		
		foreach (Transform t in propertyDetail.transform) {
			if (t.name == "ServyProperty")
				t.gameObject.SetActive(true);
			else
				t.gameObject.SetActive(false);
		}
		
		_selectedCharactor = eCharactor.SERVY;
		Actor actor = ActorManager.Instance.Get (currentGuid);
		if (actor != null)
		{
			UILabel label = ActorName.GetComponentInChildren<UILabel> () as UILabel;
			if (label != null)
			{
				label.text = actor.ActorName;
			}
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
		switch (_selectedCharactor) 
		{
			case eCharactor.BUTTY:
			{
				GameObject gameObject = GameObject.Find ("ButtyProperty");
				if (gameObject != null)
				{
					UIPopupList list = gameObject.GetComponentInChildren<UIPopupList> () as UIPopupList;
					int pin = ConvertPin(list.value);
					((Butty)actor).AttachPin(pin);
					Debug.Log ("Attach pin:" + pin);
				}
				break;
			}
			case eCharactor.BULBY:
			{
				GameObject gameObject = GameObject.Find ("BulbyProperty");
				if (gameObject != null)
				{
					int pinRed = -1;
					int pinGreen = -1;
					int pinBlue = -1;
					UIPopupList[] lists = gameObject.GetComponentsInChildren<UIPopupList> () as UIPopupList[];
					foreach (UIPopupList list in lists)
					{
						switch (list.name)
						{
							case "3.SelectRed":
							{
								pinRed = ConvertPin(list.value);
								break;
							}
							case "4.SelectGreen":
							{
								pinGreen = ConvertPin(list.value);
								break;
							}
							case "5.SelectBlue":
							{
								pinBlue = ConvertPin(list.value);
								break;
							}
						}
					}
					((Bulby)actor).AttachPins(pinRed, pinGreen, pinBlue);
					Debug.Log ("Attach pin ("+pinRed+", "+pinGreen+", "+pinBlue+")");
				}
				break;
			}
			case eCharactor.SERVY:
			{
				GameObject gameObject = GameObject.Find ("ServyProperty");
				if (gameObject != null)
				{
					UIPopupList list = gameObject.GetComponentInChildren<UIPopupList> () as UIPopupList;
					int pin = ConvertPin(list.value);
					((Servy)actor).AttachPin(pin);
					Debug.Log ("Attach pin:" + pin);
				}
				break;
			}
		}
	}

	int ConvertPin(string pinName)
	{
		if (pinName == "X")
			return -1;

		if (pinName.Length==2)
			return System.Convert.ToInt32(pinName.Substring(1));
		else
			return System.Convert.ToInt32(pinName.Substring(1,2));
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
