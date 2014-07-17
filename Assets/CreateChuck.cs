using UnityEngine;
using System.Collections;

public class CreateChuck : MonoBehaviour {
	public GameObject chuckPrefab;

	public void DisableAnchor()
	{
		Debug.Log ("DisableAnchor");
		UISprite sprite = this.GetComponentInChildren<UISprite>();
		sprite.ResetAnchors();
	}

	private Vector3 CurrentNGUIMousePosition()
	{
		float x = Input.mousePosition.x - Screen.width/2;
		float y = Input.mousePosition.y - Screen.height/2;
		return new Vector3(x, y, 0f);
	}

	public void Create()
	{
		GameObject gameObject = (GameObject)Instantiate(chuckPrefab,CurrentNGUIMousePosition(),Quaternion.identity);
		gameObject.transform.localPosition = CurrentNGUIMousePosition();
		Debug.Log (gameObject.name + gameObject.transform.localPosition.ToString());
	}
}
