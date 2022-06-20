using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
[CustomEditor(typeof(Room))]
public class RoomCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
       
        Room levelGen = (Room)target;
        if (GUILayout.Button("Bake Neighbours"))
        {
            levelGen.GetNeightboursLinealy();
        }
        if (GUILayout.Button("Remove Neighbours"))
        {
            levelGen.ClearData();
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(levelGen);
            EditorSceneManager.MarkSceneDirty(levelGen.gameObject.scene);
        }
        serializedObject.ApplyModifiedProperties();

        DrawDefaultInspector();
    }
}
