using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
	public string DestroyTag;

	public System.Guid Guid;
	public string ActorName;
	public Vector3 Pos;

	public void Start ()
	{
		Guid = System.Guid.NewGuid ();
	}

	public virtual void Refresh ()
	{
//		Debug.Log ("Actor::Refresh");
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == DestroyTag)
		{
			ActorManager.Instance.Remove(this);
			Debug.Log ("Destroy " + gameObject.name);
			Destroy(this.gameObject);
		}
	}
}
