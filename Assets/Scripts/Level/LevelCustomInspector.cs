using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(LevelGenerator))]
public class LevelCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LevelGenerator levelGen = (LevelGenerator)target;
        if (GUILayout.Button("Bake Neighbours"))
        {
            levelGen.BakeLevels();
        }
        if (GUILayout.Button("Remove Neighbours"))
        {
            levelGen.ResetLevelData();
        }
        if(GUILayout.Button("Bake Level"))
        {
            levelGen.RunLevelRoullete();
        }
    }
}
