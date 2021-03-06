﻿using UnityEngine;
using System.Collections;

public class ButtyProperty : MonoBehaviour {

	private static ButtyProperty _instance = null;
	public static ButtyProperty Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(ButtyProperty)) as ButtyProperty;
			}
			return _instance;
		}
	}

}
