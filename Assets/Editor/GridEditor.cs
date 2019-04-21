using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("GenerateGrid", GUILayout.Width(150)))
        {
            Grid grid = target as Grid;
            grid.GenerateGrid();
        }
    }
}