using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryHolder : Inventory
{
    List<Inventory> inventories;

    public InventoryHolder() : base(0, false)
    {
        inventories = new List<Inventory>();
    }

    public void AddInventory(Inventory i)
    {
        if (inventories.Contains(i)) return;

        // Combine items with this inventory
        items = items.Concat(i.Items).ToList();

        // Add to the list of inventories
        inventories.Add(i);

        for (int k = 0; k < i.Size; k++)
        {
            // Update the crafting UI to compensate for the addition of this inventory
            ModifiedItem(i.GetItem(k));
        }

        // Subscribe so that when an item is modified in
        // that inventory, notify this inventory.
        i.ItemChanged += ModifiedItem;
    }

    public void RemoveInventory(Inventory inv)
    {
        int itemsPassed = 0;

        for (int i = 0; i < inventories.Count; i++)
        {
            if (inv == inventories[i])
            {
                for (int k = 0; k < inv.Size; k++)
                {
                    // Update the crafting UI to compensate for the removal of this inventory
                    ModifiedItem(inv.GetItem(k));

                    // Remove the items from this inventory
                    items.RemoveAt((itemsPassed));
                }

                inventories.RemoveAt(i);
                break;
            }
            else
            {
                itemsPassed += inventories[i].Size;
            }
        }

        inv.ItemChanged -= ModifiedItem;
    }
}
