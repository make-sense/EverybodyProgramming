using UnityEngine;
using System.Collections;

public class PropertyManager : MonoBehaviour {

	public GameObject ActorName;
	public GameObject propertyRoot;
	public GameObject propertyDetail;

	public System.Guid currentGuid;

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

	public void ShowProperty (System.Guid guid) {
		currentGuid = guid;
		Show ();

		try {
			Actor actor = ActorManager.Instance.Get (currentGuid);
			foreach (Transform t in propertyDetail.transform) {
				if (t.name == actor.GetType ().ToString ())
					t.gameObject.SetActive(true);
				else
					t.gameObject.SetActive(false);
			}
			UIInput input = ActorName.GetComponentInChildren<UIInput> () as UIInput;
			input.value = actor.GetName ();
		}
		catch (System.NullReferenceException e) 
		{
			Debug.Log (e.ToString ());
		}
	}

	public void Show () {
		propertyRoot.SetActive(true);
	}

	public void Hide () {
		propertyRoot.SetActive(false);
	}

	public void Set () {
		UIInput input = ActorName.GetComponentInChildren<UIInput> () as UIInput;
//		UILabel label = ActorName.GetComponentInChildren<UILabel> () as UILabel;
		Actor actor = ActorManager.Instance.Get (currentGuid);
		actor.SetName (input.value);
		switch (actor.GetType ().ToString ()) 
		{
			case "Butty":
			{
				GameObject gameObject = GameObject.Find (actor.GetType ().ToString ());
				if (gameObject != null)
				{
					UIPopupList list = gameObject.GetComponentInChildren<UIPopupList> () as UIPopupList;
					int pin = ConvertPin(list.value);
					((Butty)actor).AttachPin(pin);
					Debug.Log ("Attach pin:" + pin);
				}
				break;
			}
			case "Bulby":
			{
				GameObject gameObject = GameObject.Find (actor.GetType ().ToString ());
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
			case "Servy":
			{
				GameObject gameObject = GameObject.Find (actor.GetType ().ToString ());
				if (gameObject != null)
				{
					UIPopupList list = gameObject.GetComponentInChildren<UIPopupList> () as UIPopupList;
					int pin = ConvertPin(list.value);
					((Servy)actor).AttachPin(pin);
					Debug.Log ("Attach pin:" + pin);
				}
				break;
			}
			case "DcMotor":
			{
				GameObject gameObject = GameObject.Find (actor.GetType ().ToString ());
				if (gameObject != null)
				{
					UIPopupList list = gameObject.GetComponentInChildren<UIPopupList> () as UIPopupList;
					int pin = ConvertPin(list.value);
					((DcMotor)actor).AttachPin(pin);
					Debug.Log ("Attach pin:" + pin);
				}
				break;
			}
			case "Analog":
			{
				GameObject gameObject = GameObject.Find (actor.GetType ().ToString ());
				if (gameObject != null)
				{
					UIPopupList list = gameObject.GetComponentInChildren<UIPopupList> () as UIPopupList;
					int pin = ConvertPin(list.value);
					((Analog)actor).AttachPin(pin);
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
