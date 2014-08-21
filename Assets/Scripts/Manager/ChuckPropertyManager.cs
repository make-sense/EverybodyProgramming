using UnityEngine;
using System.Collections;

public class ChuckPropertyManager : MonoBehaviour {

	public GameObject propertyRoot;
	public GameObject actorScrollRoot;
	public GameObject actionScrollRoot;

	public GameObject actorSymbolPrefab;
	public GameObject actionButtonPrefab;

	public static System.Guid SelectedChuckGuid;
	public static System.Guid SelectedActorGuid;

	// Use this for initialization
	void Start () {
		Hide ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Show () {
		propertyRoot.SetActive(true);
		UpdateActors ();
		DeleteActionButton ();
	}
	
	public void Hide () {
		propertyRoot.SetActive(false);
	}

	private void DeleteActorSymbol () {
		MsUtil.DeleteChildren(actorScrollRoot);
	}

	public void UpdateActors() {
		DeleteActorSymbol ();

		BetterList<Actor> actors = ActorManager.Instance.GetActors();
		int count = 0;
		int baseX = -65;
		int width = 50;
		foreach (Actor actor in actors) {
			GameObject instantiatedGameObject = NGUITools.AddChild(actorScrollRoot, actorSymbolPrefab);
			instantiatedGameObject.transform.localPosition = new Vector3((float)baseX+width*count, 0f, 0f);

			ActorSymbol actorSymbol = instantiatedGameObject.GetComponentInChildren<ActorSymbol> () as ActorSymbol;
			actorSymbol.guid = actor.Guid;

			UIButton button = instantiatedGameObject.GetComponentInChildren<UIButton> () as UIButton;
			switch (actor.GetType ().ToString ()) {
				case "Stage":
					button.normalSprite = "1408105883_traffic_lights_green";
					break;
				case "Butty":
					button.normalSprite = "Button";
					break;
				case "Bulby":
					button.normalSprite = "1407588716_bulb";
					break;
				case "Sandy":
					button.normalSprite = "1408611113_sandglass";
					break;
			}
			count++;
		}
	}

	void DeleteActionButton () {
		MsUtil.DeleteChildren(actionScrollRoot);
	}

	public void UpdateAction(System.Guid actorGuid) {
		DeleteActionButton ();

		SelectedActorGuid = actorGuid;

		Debug.Log ("ChuckPropertyManager::UpdateAction with " + actorGuid);
		Actor actor = ActorManager.Instance.Get (actorGuid);
		if (actor != null) {
			int count = 0;
			int baseY = 70;
			int heightStep = -40;
			switch (actor.GetType ().ToString ())
			{
				case "Stage":
				{
					ActionTable actions = GameObject.Find ("ActionStage").GetComponentInChildren<ActionTable> () as ActionTable;
					foreach (ActionData data in actions.DataList)
					{
						GameObject instantiatedGO = NGUITools.AddChild(actionScrollRoot, actionButtonPrefab);
						instantiatedGO.transform.localPosition = new Vector3(0f, (float)baseY+heightStep*count, 0f);
						UILabel label = instantiatedGO.GetComponentInChildren<UILabel> () as UILabel;
						label.text = data.Name;
						ActionSymbol action = instantiatedGO.GetComponentInChildren<ActionSymbol> () as ActionSymbol;
						action.guid = data.Guid;
						count++;
					}
					break;
				}
				case "Butty":
				{
					ActionTable actions = GameObject.Find ("ActionButty").GetComponentInChildren<ActionTable> () as ActionTable;
					foreach (ActionData data in actions.DataList)
					{
						GameObject instantiatedGO = NGUITools.AddChild(actionScrollRoot, actionButtonPrefab);
						instantiatedGO.transform.localPosition = new Vector3(0f, (float)baseY+heightStep*count, 0f);
						UILabel label = instantiatedGO.GetComponentInChildren<UILabel> () as UILabel;
						label.text = data.Name;
						ActionSymbol action = instantiatedGO.GetComponentInChildren<ActionSymbol> () as ActionSymbol;
						action.guid = data.Guid;
						count++;
					}
					break;
				}
				case "Bulby":
				{
					ActionTable actions = GameObject.Find ("ActionBulby").GetComponentInChildren<ActionTable> () as ActionTable;
					foreach (ActionData data in actions.DataList)
					{
						GameObject instantiatedGO = NGUITools.AddChild(actionScrollRoot, actionButtonPrefab);
						instantiatedGO.transform.localPosition = new Vector3(0f, (float)baseY+heightStep*count, 0f);
						UILabel label = instantiatedGO.GetComponentInChildren<UILabel> () as UILabel;
						label.text = data.Name;
						ActionSymbol action = instantiatedGO.GetComponentInChildren<ActionSymbol> () as ActionSymbol;
						action.guid = data.Guid;
						count++;
					}
					break;
				}
				case "Sandy":
				{
				ActionTable actions = GameObject.Find ("ActionSandy").GetComponentInChildren<ActionTable> () as ActionTable;
				foreach (ActionData data in actions.DataList)
				{
					GameObject instantiatedGO = NGUITools.AddChild(actionScrollRoot, actionButtonPrefab);
					instantiatedGO.transform.localPosition = new Vector3(0f, (float)baseY+heightStep*count, 0f);
					UILabel label = instantiatedGO.GetComponentInChildren<UILabel> () as UILabel;
					label.text = data.Name;
					ActionSymbol action = instantiatedGO.GetComponentInChildren<ActionSymbol> () as ActionSymbol;
					action.guid = data.Guid;
					count++;
				}
				break;
				}
			}
		}
	}

	public void SetCurrentAction(int actionGuid) {
		Chuck chuck = ChuckManager.Instance.Get (SelectedChuckGuid);
		if (chuck != null) {
			chuck.SetAction(SelectedActorGuid, actionGuid);
		}
		Debug.Log ("SetCurrentAction:" + actionGuid.ToString ());
	}

	private static ChuckPropertyManager _instance = null;
	public static ChuckPropertyManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(ChuckPropertyManager)) as ChuckPropertyManager;
			}
			return _instance;
		}
	}
}
