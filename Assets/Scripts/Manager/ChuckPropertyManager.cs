using UnityEngine;
using System.Collections;

public class ChuckPropertyManager : MonoBehaviour {

	public GameObject propertyRoot;
	public GameObject actorScrollRoot;
	public GameObject actionScrollRoot;

	public GameObject actorSymbolPrefab;
	public GameObject actionButtonPrefab;

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

			switch (actor.charactorType) {
				case Actor.eCharactor.BUTTY:
				{
					UISprite sprite = instantiatedGameObject.GetComponentInChildren<UISprite> () as UISprite;
					sprite.spriteName = "Button";
					break;
				}
				case Actor.eCharactor.BULBY:
				{
					UISprite sprite = instantiatedGameObject.GetComponentInChildren<UISprite> () as UISprite;
					sprite.spriteName = "Bulb";
					break;
				}
			}
			count++;
		}
	}

	void DeleteActionButton () {
		MsUtil.DeleteChildren(actionScrollRoot);
	}

	public void UpdateAction(System.Guid actorGuid) {
		DeleteActionButton ();

		Debug.Log ("ChuckPropertyManager::UpdateAction with " + actorGuid);
		Actor actor = ActorManager.Instance.Get (actorGuid);
		if (actor != null) {
			int count = 0;
			int baseY = 70;
			int height = -40;
			switch (actor.charactorType)
			{
				case Actor.eCharactor.BUTTY:
				{
					ActionButty actions = GameObject.Find ("ActionButty").GetComponentInChildren<ActionButty> () as ActionButty;
					foreach (ActionData data in actions.DataList)
					{
						GameObject instantiatedGO = NGUITools.AddChild(actionScrollRoot, actionButtonPrefab);
						instantiatedGO.transform.localPosition = new Vector3(0f, (float)baseY+height*count, 0f);
						UILabel label = instantiatedGO.GetComponentInChildren<UILabel> () as UILabel;
						label.text = data.Name;
						count++;
					}
					break;
				}
				case Actor.eCharactor.BULBY:
				{
					ActionBulby actions = GameObject.Find ("ActionBulby").GetComponentInChildren<ActionBulby> () as ActionBulby;
					foreach (ActionData data in actions.DataList)
					{
						GameObject instantiatedGO = NGUITools.AddChild(actionScrollRoot, actionButtonPrefab);
						instantiatedGO.transform.localPosition = new Vector3(0f, (float)baseY+height*count, 0f);
						UILabel label = instantiatedGO.GetComponentInChildren<UILabel> () as UILabel;
						label.text = data.Name;
						count++;
					}
					break;
				}
			}
		}
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
