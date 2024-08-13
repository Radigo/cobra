using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BuildRiver))]
public class PathInspector : Editor
{
    public override void OnInspectorGUI()
    {
        /*
        BuildRiver myTarget = (BuildRiver)target;

        myTarget.width = EditorGUILayout.FloatField(myTarget.width);
        EditorGUILayout.LabelField("Width", myTarget.width.ToString());
        */

        DrawDefaultInspector();

        BuildRiver myTarget = (BuildRiver)target;
        
        if(GUILayout.Button("Build Object"))
        {
            myTarget.BuildRiverMesh();
        }
    }
}