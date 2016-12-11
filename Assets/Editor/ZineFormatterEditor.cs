using UnityEditor;
using UnityEngine;
using Rotorz.ReorderableList;
using System.Collections;

[CustomEditor(typeof(ZineFormatter))]
public class ZineFormatterEditor : Editor
{
	SerializedProperty _pagesProperty;

	void OnEnable()
	{
		_pagesProperty = serializedObject.FindProperty("_pages");
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		serializedObject.Update();

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Pages", EditorStyles.centeredGreyMiniLabel);
		ReorderableListGUI.ListField(_pagesProperty, ReorderableListFlags.ShowIndices);

		serializedObject.ApplyModifiedProperties();
	}
}
