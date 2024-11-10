using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using Unity.VisualScripting;

[CustomEditor(typeof(InventoryManager))]
public class InventoryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // SerializedProperty obstacleLocationProperty = serializedObject.FindProperty("NewObstacleLocation");

        // if (obstacleLocationProperty != null)
        // {
        //     EditorGUILayout.PropertyField(obstacleLocationProperty);
        //     obstacleLocationProperty.serializedObject.ApplyModifiedProperties();
        // }

        // // Draw selection GUI in horizontal pattern
        // using (new EditorGUILayout.HorizontalScope())
        // {
        //     Pathfinding pathfinding = (Pathfinding)serializedObject.targetObject;
        //     if (GUILayout.Button("Place obstacle"))
        //     {
        //         pathfinding.AddObstacle(obstacleLocationProperty.vector2IntValue);
        //     }

        //     if (GUILayout.Button("Generate new grid"))
        //     {
        //         pathfinding.GenerateNewGrid();
        //     }
        // }

        
        InventoryManager inventory = (InventoryManager)serializedObject.targetObject;
        SerializedProperty invItemNameProperty = serializedObject.FindProperty("InvItemName");
        SerializedProperty invItemIDProperty = serializedObject.FindProperty("InvItemID");

        if (invItemNameProperty != null)
        {
            EditorGUILayout.PropertyField(invItemNameProperty);
            invItemNameProperty.serializedObject.ApplyModifiedProperties();
        }

        if (invItemIDProperty != null)
        {
            EditorGUILayout.PropertyField(invItemIDProperty);
            invItemIDProperty.serializedObject.ApplyModifiedProperties();
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Search item by name"))
            {
                string nameToSearch = invItemNameProperty.stringValue;
                InventoryItem item = inventory.LinearSearchByName(nameToSearch);
                StringBuilder output = new StringBuilder();
                output.Append($"Searching for {nameToSearch}... ");

                if(item != null)
                    output.Append("Found with linear search!");
                else
                    output.Append($"{nameToSearch} not found.");

                Debug.Log(output.ToString());
            }

            if (GUILayout.Button("Search item by ID"))
            {
                int IDToSearch = invItemIDProperty.intValue;
                InventoryItem item = inventory.BinarySearchByID(IDToSearch);
                StringBuilder output = new StringBuilder();
                output.Append($"Searching for #{IDToSearch}... ");

                if(item != null)
                    output.Append("Found with linear search!");
                else
                    output.Append($"#{IDToSearch} not found.");

                Debug.Log(output.ToString());
            }
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Sort inventory values"))
            {
                inventory.QuickSortInventory();
            }

            if (GUILayout.Button("Shuffle inventory"))
            {
                inventory.ShuffleInventory();
            }
        }

        if (GUILayout.Button("Generate new inventory"))
        {
            inventory.RegenerateInventory();
        }

    }
}
