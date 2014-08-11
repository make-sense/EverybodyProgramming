//--------------------------------------------------------------------------------
// Author	   : 김재현
// Date		   : 2012-08-14
// Copyright   : 2011-2014 Hansung Univ. Robots in Education & Entertainment Lab.
//
// Description : SiCiEditorBase
//
//--------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEditor;

public class SiCiUnityEditorBase : Editor
{
	protected bool Button(string text, float width = 100f, float height = 20f, bool expand = true)
	{
		return GUILayout.Button(text ,GUILayout.Width( width ) ,GUILayout.Height( height ) ,GUILayout.ExpandWidth( expand ));
	}
}
