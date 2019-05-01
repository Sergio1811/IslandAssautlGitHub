using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    public int characterNumber;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        characterNumber = EditorGUILayout.IntSlider(characterNumber, 0, 2);

        if (GUILayout.Button("GenerateGrid", GUILayout.Width(150)))
        {
            Grid grid = target as Grid;
            grid.GenerateGrid(characterNumber);
        }
    }
}