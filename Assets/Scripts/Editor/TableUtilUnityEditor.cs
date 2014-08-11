//--------------------------------------------------------------------------------
// Author	   : 김재현
// Date		   : 2012-08-14
// Copyright   : 2011-2014 Hansung Univ. Robots in Education & Entertainment Lab.
//
// Description : TableUtilEditor
//
//--------------------------------------------------------------------------------

using System.Collections;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


[CustomEditor(typeof(TableUtil))]
public class TableUtilUnityEditor : SiCiUnityEditorBase
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		TableUtil util = target as TableUtil;

		if(util == null)
			return;

		if(Button("Guid 생성"))
		{
			util.newGuid = MsUtil.NewGuid();
		}
	}


	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
