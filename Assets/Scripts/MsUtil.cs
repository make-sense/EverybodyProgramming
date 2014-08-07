using UnityEngine;
using System.Collections;

public class MsUtil : MonoBehaviour {
	public static void DeleteChildren(GameObject gameObject) 
	{
		foreach (Transform child in gameObject.transform)
			Destroy (child.gameObject);
	}
}
