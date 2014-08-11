using UnityEngine;
using System.Collections;

public class ActionSymbol : MonoBehaviour {

	public int guid;
	
	void OnClick () {
		Debug.Log ("ActionSymbol::OnClick");
		ChuckPropertyManager.Instance.SetCurrentAction(guid);
	}
}
