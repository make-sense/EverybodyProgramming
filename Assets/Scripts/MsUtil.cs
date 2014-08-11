using UnityEngine;
using System.Collections;

public class MsUtil : MonoBehaviour {
	public static void DeleteChildren(GameObject gameObject) 
	{
		foreach (Transform child in gameObject.transform)
			Destroy (child.gameObject);
	}

	public static int NewGuid()
	{
		return System.Convert.ToInt32(System.Guid.NewGuid().ToString("N"), 16);
	}
}
