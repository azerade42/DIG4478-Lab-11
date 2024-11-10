using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] int numInventoryItems;
    [SerializeField] int rowLength;
    [SerializeField] float spacing;
    [SerializeField] private InventoryItem item;

    [HideInInspector] public string InvItemName;
    [HideInInspector] public int InvItemID;

    private List<InventoryItem> inventory = new List<InventoryItem>();

    private void Start()
    {
        GenerateRandomInventory();
    }

    public void GenerateRandomInventory()
    {
        int xPos = 0;
        int yPos = 0;

        for (int i = 0; i < numInventoryItems; i++)
        {
            Vector3 itemPos = new Vector3(xPos * spacing, yPos * spacing, 0);
            InventoryItem newItem = Instantiate(item, itemPos, Quaternion.identity, transform).GetComponent<InventoryItem>();

            int itemID = i;
            string itemName = GenerateRandomString(10);
            int itemValue = Random.Range(0, 100);
            newItem.gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(itemValue*0.01f, itemValue*0.01f, itemValue*0.01f));

            newItem.OnInstantiate(itemID, itemName, itemValue);
            inventory.Add(newItem);

            if (++xPos > rowLength - 1)
            {
                yPos--;
                xPos = 0;
            }
        }
    }

    public void ShuffleInventory()
    {
        inventory = inventory.OrderBy(x => Random.value).ToList();
        RepositionInventory();
    }

    public void RepositionInventory()
    {
        int xPos = 0;
        int yPos = 0;

        foreach (InventoryItem item in inventory)
        {
            Vector3 itemPos = new Vector3(xPos * spacing, yPos * spacing, 0);
            item.transform.position = itemPos;

            if (++xPos > rowLength - 1)
            {
                yPos--;
                xPos = 0;
            }
        }
    }

    public void RegenerateInventory()
    {
        foreach (InventoryItem item in inventory)
        {
            Destroy(item.gameObject);
        }
        
        inventory.Clear();
        GenerateRandomInventory();
    }

    public string GenerateRandomString(int length)
    {
        StringBuilder str = new StringBuilder();
        string chars = "abcdefghijklmnopqrstuvwxyz";
    
        for (int i = 0; i < length; i++) 
        {
            char @char = chars[Random.Range(0, chars.Length)];
            str.Append(@char);
        }

        return str.ToString();
    }

    public InventoryItem LinearSearchByName(string itemName)
    {
        foreach (InventoryItem item in inventory)
        {
            if (item.Name.CompareTo(itemName) == 0)
                return item;
        }

        return null;
    }

    public InventoryItem BinarySearchByID(int ID)
    {
        SortInventoryByID();

        int low = 0;
        int high = inventory.Count - 1;

        while (low <= high)
        {
            int mid = low + (high - low) / 2;

            if (inventory[mid].ID == ID)
            {
                return inventory[mid];
            }
            else if (inventory[mid].ID < ID)
            {
                low = mid + 1;
            }
            else
            {
                high = mid - 1;
            }
        }

        return null;
    }

    private int Partition(int first, int last)
    {
        int pivot = inventory[last].Value;
        int smaller = first - 1;

        for (int element = first; element < last; element++)
        {
            if (inventory[element].Value < pivot)
            {
                smaller++;

                InventoryItem temporary = inventory[smaller];
                inventory[smaller] = inventory[element];
                inventory[element] = temporary;
            }
        }

        InventoryItem temporaryNext = inventory[smaller + 1];
        inventory[smaller + 1] = inventory[last];
        inventory[last] = temporaryNext;

        return smaller + 1;
    }

    private void QuickSortByValue(int first, int last)
    {
        if (first < last)
        {
            int pivot = Partition(first, last);

            QuickSortByValue(first, pivot - 1);
            QuickSortByValue(pivot + 1, last);
        }

    }

    public void SortInventoryByValue()
    {
        QuickSortByValue(0, inventory.Count - 1);
        
        RepositionInventory();
    }

    private void SortInventoryByID()
    {
        inventory = inventory.OrderBy(x => x.ID).ToList();

        RepositionInventory();
    }

    public string PrintAllValues()
    {
        StringBuilder stringBuilder = new StringBuilder();

        foreach (InventoryItem item in inventory)
        {
            stringBuilder.Append(item.Value).Append(" ");
        }

        return stringBuilder.ToString();
    }
}
