using UnityEngine;
using System.Collections;

public class ActionSymbol : MonoBehaviour {

	public int guid;

	void OnClick () {
		Debug.Log ("ActionSymbol::OnClick");
		if (transform.parent.name.Contains ("ActionParam1"))
		{
			UIInput input = transform.parent.GetComponentInChildren<UIInput> () as UIInput;
			ChuckPropertyManager.Instance.SetCurrentAction(guid, input.value);
		}
		else
		{
			ChuckPropertyManager.Instance.SetCurrentAction(guid);
		}
	}
}
