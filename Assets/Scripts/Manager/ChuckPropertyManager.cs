using UnityEngine;
using System.Collections;

public class ChuckPropertyManager : MonoBehaviour {

	public GameObject propertyRoot;
	public GameObject actorScrollRoot;
	public GameObject actionScrollRoot;

	public GameObject actorSymbolPrefab;

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
	}
	
	public void Hide () {
		propertyRoot.SetActive(false);
	}

	public void UpdateActors() {
		foreach (Transform child in actorScrollRoot.transform)
			Destroy (child.gameObject);
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

	public void UpdateAction(System.Guid actorGuid) {
		Debug.Log ("ChuckPropertyManager::UpdateAction with " + actorGuid);
		Actor actor = ActorManager.Instance.Get (actorGuid);
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
