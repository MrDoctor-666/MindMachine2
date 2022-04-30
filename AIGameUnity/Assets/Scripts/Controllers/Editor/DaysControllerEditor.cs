using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DaysController))]
public class DaysControllerEditor : Editor
{

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUILayout.LabelField("General Game Info", EditorStyles.boldLabel);
		EditorGUILayout.LabelField("Computing Power", GameInfo.computingPower.ToString());
		EditorGUILayout.LabelField("Suspicion", GameInfo.suspicion.ToString());


		DaysController myScript = (DaysController)target;
		
		if (GUILayout.Button("Next Day"))
		{
			EventAggregator.changeDay.Publish();
		}
	}
}