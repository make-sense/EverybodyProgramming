using UnityEngine;
using System.Collections;

public class Stage : Actor {

	public void ToggleRunStop ()
	{
		_isRun = !_isRun;
		SetUI ();
	}

	public void Run ()
	{
		_isRun = true;
		SetUI ();
	}

	public void Stop ()
	{
		_isRun = false;
		SetUI ();
	}

	public bool IsRun ()
	{
		return _isRun;
	}

	private void SetUI ()
	{
		UIButton button = GetComponentInChildren<UIButton> () as UIButton;
		if (button == null)
			return;

		if (_isRun) 
			button.normalSprite = "1408105883_traffic_lights_green";
		else 
			button.normalSprite = "1408105883_traffic_lights_red";
	}

	private bool _isRun = false;

	// Use this for initialization
	void Start () {
		base.Start ();
		base.charactorType = eCharactor.STAGE;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
