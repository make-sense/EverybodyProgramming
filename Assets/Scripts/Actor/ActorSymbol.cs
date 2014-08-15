using UnityEngine;
using System.Collections;

public class ActorSymbol : MonoBehaviour {

	public System.Guid guid;

	void OnClick () {
		Debug.Log ("ActorSymbol::OnClick");
		ChuckPropertyManager.Instance.UpdateAction (guid);
	}
}
