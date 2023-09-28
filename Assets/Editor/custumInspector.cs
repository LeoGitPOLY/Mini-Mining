using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StageBlocList))]
public class custumInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StageBlocList stage = (StageBlocList)target;

        if (GUILayout.Button("Calcul Tot"))
        {
            stage.calculTot();
        }

        if (GUILayout.Button("Resize Monde:"))
        {
            stage.resizeMonde();
        }
    }
}
