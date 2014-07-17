using UnityEngine;
using System.Collections;

public class CreateChuck : MonoBehaviour {
	public GameObject chuckPrefab;
	private static Vector3 _lastPosition;

	public void DisableAnchor()
	{
		Debug.Log ("DisableAnchor");
		UISprite sprite = this.GetComponentInChildren<UISprite>();
		sprite.ResetAnchors();
	}

	void OnDrag (Vector2 delta)
	{
		Transform _transform = UIRoot.list[0].transform.FindChild("ChuckStack(Clone)");
		if (_transform != null) {
			_lastPosition = _transform.localPosition;
			Debug.Log(_transform.name + ":" + _lastPosition.ToString());
		}
	}

	public void Create()
	{
		GameObject instantiatedGameObject = NGUITools.AddChild(this.transform.parent.gameObject, chuckPrefab);
		instantiatedGameObject.transform.localPosition = _lastPosition;
	}
}
