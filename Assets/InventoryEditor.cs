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
                Debug.Log($"Searching for {nameToSearch}... ");

                if(item != null)
                    Debug.Log($"Found {item.Name} with linear search!");
                else
                    Debug.LogWarning($"{nameToSearch} not found.");
            }

            if (GUILayout.Button("Search item by ID"))
            {
                int IDToSearch = invItemIDProperty.intValue;
                InventoryItem item = inventory.BinarySearchByID(IDToSearch);
                Debug.Log($"Searching for #{IDToSearch}... ");

                if(item != null)
                    Debug.Log($"Found #{item.ID} with binary search!");
                else
                    Debug.LogWarning($"#{IDToSearch} not found.");
            }
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Sort inventory values"))
            {
                inventory.SortInventoryByValue();
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
