﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorManager : MonoBehaviour {
	BetterList<Actor> _actors = new BetterList<Actor> ();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Add (Actor actor) {
		_actors.Add(actor);
	}

	public int Count () {
		return _actors.size;
	}

	public Actor Get(System.Guid guid) {
		foreach (Actor actor in _actors) {
			if (actor.Guid == guid)
				return actor;
		}
		return null;
	}

	private static ActorManager _instance = null;
	public static ActorManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(ActorManager)) as ActorManager;
			}
			return _instance;
		}
	}

}