﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChuckManager : MonoBehaviour {

	BetterList<Chuck> _chucks = new BetterList<Chuck> ();

	public int count;

	void Update () {
		count = _chucks.size;
	}

	public void Add (Chuck chuck) {
		_chucks.Add(chuck);
	}

	public void Remove (Chuck chuck) {
		_chucks.Remove(chuck);
	}

	public int Count () {
		return _chucks.size;
	}
	
	public Chuck Get(System.Guid guid) {
		foreach (Chuck chuck in _chucks) {
			if (chuck.Guid == guid)
				return chuck;
		}
		return null;
	}
	
	public BetterList<Chuck> GetChucks() {
		return _chucks;
	}

	private static ChuckManager _instance = null;
	public static ChuckManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(ChuckManager)) as ChuckManager;
			}
			return _instance;
		}
	}
}
