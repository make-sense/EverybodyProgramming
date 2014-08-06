using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
	public string DestroyTag;

	public enum eCharactor {
		NONE,
		BUTTY,
		CALCHY,
		BULBY,
	};
	public eCharactor charactorType = eCharactor.NONE;

	public System.Guid Guid;
	public string ActorName;
	public Vector3 Pos;

	public void Start ()
	{
		Guid = System.Guid.NewGuid ();
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == DestroyTag)
		{
			Debug.Log ("Destroy " + gameObject.name);
			Destroy(this.gameObject);
		}
	}
}
