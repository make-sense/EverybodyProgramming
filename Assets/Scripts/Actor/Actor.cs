using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
	public string DestroyTag;

	public System.Guid Guid;
	public string _Name;
	public Vector3 Pos;
	public GameObject action;

	public void Start ()
	{
		Guid = System.Guid.NewGuid ();
	}

	public virtual void Refresh ()
	{
//		Debug.Log ("Actor::Refresh");
	}

	public string GetName ()
	{
		return _Name;
	}

	public void SetName (string name)
	{
		GameObject nameObject = transform.FindChild("Name").gameObject;
		if (name.Length > 0)
		{
			nameObject.SetActive(true);
			UILabel label = nameObject.GetComponentInChildren<UILabel> () as UILabel;
			label.text = name;
			_Name = name;
		}
		else
		{
			nameObject.SetActive(false);
		}
	}

//	public virtual void SetName (string name)
//	{
//	}

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
