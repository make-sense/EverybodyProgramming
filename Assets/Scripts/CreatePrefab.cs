using UnityEngine;
using System.Collections;

public class CreatePrefab : MonoBehaviour {
	public Transform ActorRoot;
	public GameObject positionOfPrefab;
	public GameObject prefab;
	private static Vector3 _lastPosition;
	
	void OnDrag (Vector2 delta)
	{
		Transform _transform = ActorRoot.FindChild(positionOfPrefab.name);
		if (_transform != null) {
			_lastPosition = _transform.localPosition;
//			Debug.Log(_transform.name + ":" + _lastPosition.ToString());
		}
	}
	
	public void Create()
	{
		GameObject instantiatedGameObject = NGUITools.AddChild(this.transform.parent.gameObject, prefab);
		instantiatedGameObject.transform.localPosition = _lastPosition;
		UISprite sprite = instantiatedGameObject.GetComponentInChildren<UISprite> () as UISprite;
		sprite.MakePixelPerfect ();
	}
}
