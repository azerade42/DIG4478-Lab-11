using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [field:SerializeField] public int ID { get; private set; }
    [field:SerializeField] public string Name { get; private set; }
    public int Value;

    public void OnInstantiate(int ID, string Name, int Value)
    {
        this.ID = ID;
        this.Name = Name;
        this.Value = Value;
    }
}
