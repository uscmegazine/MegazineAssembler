using UnityEditor;
using UnityEngine;
using Rotorz.ReorderableList;
using System.Collections;

[CustomEditor(typeof(PageCollection))]
public class PageCollectionEditor : Editor
{
	SerializedProperty _volumeProperty;
	SerializedProperty _pagesProperty;
	SerializedProperty _backCoverProperty;

	void OnEnable()
	{
		_volumeProperty = serializedObject.FindProperty("volume");
		_pagesProperty = serializedObject.FindProperty("pages");
		_backCoverProperty = serializedObject.FindProperty("backCover");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		_volumeProperty.stringValue = EditorGUILayout.TextField("Volume", _volumeProperty.stringValue);

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Pages", EditorStyles.centeredGreyMiniLabel);
		ReorderableListGUI.ListField(_pagesProperty, ReorderableListFlags.ShowIndices);

		_backCoverProperty.objectReferenceValue = EditorGUILayout.ObjectField("Back Cover", _backCoverProperty.objectReferenceValue, typeof(Sprite), false);

		serializedObject.ApplyModifiedProperties();
	}
}
