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

    private List<InventoryItem> inventory = new List<InventoryItem>();

    private void Start()
    {
        GenerateRandomInventory();

        print(PrintAllValues());
        
        // print(BinarySearchByID(0));

        // randomly shuffles inventory in memory but not visually
        inventory = inventory.OrderBy(x => Random.value).ToList();

        print(PrintAllValues());

        QuickSortByValue(inventory[0].Value, inventory[inventory.Count - 1].Value);

        print(PrintAllValues());
    }

    public void GenerateRandomInventory()
    {
        int xPos = 0;
        int yPos = 0;

        for (int i = 0; i < numInventoryItems; i++)
        {
            if (xPos++ > rowLength)
            {
                yPos++;
                xPos = 0;
            }

            Vector3 itemPos = new Vector3(xPos * spacing, yPos * spacing, 0);
            InventoryItem newItem = Instantiate(item, itemPos, Quaternion.identity).GetComponent<InventoryItem>();

            int itemID = i;
            string itemName = GenerateRandomString(10);
            int itemValue = Random.Range(0, 100);

            newItem.OnInstantiate(itemID, itemName, itemValue);
            inventory.Add(newItem);
        }
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

    private InventoryItem LinearSearchByName(string itemName)
    {
        foreach (InventoryItem item in inventory)
        {
            if (item.name == itemName)
                return item;
        }

        return null;
    }

    private InventoryItem BinarySearchByID(int ID)
    {
        int low = 0;
        int high = inventory.Count - 1;

        while (low <= high)
        {
            int mid = low + (high - low) / 2;
            print(inventory[mid].ID);

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

    public int Partition(int first, int last)
    {
        int pivot = inventory[last].Value;
        int smaller = first - 1;

        for (int element = first; element < last; element++)
        {
            if (inventory[element].Value < pivot)
            {
                element++;

                int temporary = inventory[smaller].Value;
                inventory[smaller] = inventory[element];
                inventory[element].Value = temporary;
            }
        }

        int temporaryNext = inventory[smaller + 1].Value;
        inventory[smaller + 1] = inventory[last];
        inventory[last].Value = temporaryNext;

        return smaller + 1;
    }

    public void QuickSortByValue(int first, int last)
    {
        if (first < last)
        {
            int pivot = Partition(first, last);

            QuickSortByValue(first, pivot - 1);
            QuickSortByValue(pivot + 1, last);

        }
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
