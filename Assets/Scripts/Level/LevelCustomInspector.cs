using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(LevelGenerator))]
public class LevelCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        LevelGenerator levelGen = (LevelGenerator)target;
        if (GUILayout.Button("Bake Level"))
        {
            levelGen.BakeLevels();
        }
        serializedObject.ApplyModifiedProperties();
        DrawDefaultInspector();
    }

}