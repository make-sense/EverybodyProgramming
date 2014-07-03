using UnityEngine;
using System.Collections;

public class CreateChuck : MonoBehaviour {
	public GameObject chuckPrefab;

	public void OnClick()
	{
		Instantiate(chuckPrefab,new Vector3(transform.position.x, 1.0f, transform.position.z),Quaternion.identity);
	}
}
