using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory
{
    public event Action<ItemContainer> ItemChanged;
    public event Action<ItemContainer> CountChanged;

    public int Size { get { return size; } }
    public string Name { get; set; }

    public List<ItemContainer> Items { get { return items; } }

    protected List<ItemContainer> items;

    [SerializeField] int size;

    public Inventory(int size, bool createItemContainers = true)
    {
        items = new List<ItemContainer>();

        this.size = size;

        if (createItemContainers)
            CreateItemContainers();
    }

    protected void ModifiedItem(ItemContainer i)
    {
        ItemChanged?.Invoke(i);
    }

    protected void ModifiedCount(ItemContainer i)
    {
        CountChanged?.Invoke(i);
    }


    public ItemContainer GetItem(int index)
    {
        return items[index];
    }

    void CreateItemContainers()
    {
        for (int i = 0; i < size; i++)
        {
            items.Add(new ItemContainer(null, 0, null, null, null));
            items[i].RegisterOnItemChanged(ModifiedItem);
            items[i].RegisterOnCountChanged(CountChanged);
        }
    }

    /// <returns>Count missing</returns>
    public int Contains(Item item, int count)
    {
        int countNeeded = count;

        for (int i = 0; i < items.Count; i++)
        {
            ItemContainer itemh = items[i];

            if (itemh.ItemType == item)
            {
                countNeeded -= itemh.Count;

                if (countNeeded <= 0) return 0;
            }
        }

        return countNeeded;
    }

    //public void Craft(Recipe r)
    //{
    //    for (int k = 0; k < r.Ingredients.Length; k++)
    //    {
    //        Item ingredient = r.Ingredients[k].item;
    //        int count = r.Ingredients[k].count;

    //        for (int i = 0; i < items.Count; i++)
    //        {
    //            ItemContainer itemh = items[i];

    //            if (itemh.ItemType == ingredient)
    //            {
    //                int prevCount = count;
    //                count = Mathf.Max(0, count - itemh.Count);
    //                itemh.Count -= prevCount - count;

    //                if (count <= 0) break;
    //            }
    //        }
    //    }
    //}

    public int TryAddItem(Item item, int count)
    {
        int countLeft = count;
        int emptySlot = -1;

        for (int i = 0; i < items.Count; i++)
        {
            ItemContainer holder = items[i];

            // Slot already has an item
            if (holder.ItemType != null)
            {
                // Slot has same item type in it
                if (holder.ItemType == item)
                {
                    int spaceLeft = holder.SpaceLeft();

                    // Slot has room left
                    if (spaceLeft > 0)
                    {
                        // Slot does not have enough room for all of it
                        if (spaceLeft < countLeft)
                        {
                            holder.Count += spaceLeft;
                            countLeft -= spaceLeft;
                            continue;
                        }
                        // Slot has enough room for all of it
                        else
                        {
                            holder.Count += countLeft;
                            countLeft = 0;
                            break;
                        }
                    }

                    continue;
                }
                else
                    continue;
            }
            // Slot is empty
            else
            {
                if (emptySlot == -1)
                    emptySlot = i;
            }
        }

        if (countLeft > 0 && emptySlot != -1)
        {
            ItemContainer ih = items[emptySlot];
            ih.ItemType = item;
            ih.Count += countLeft;
            countLeft = 0;
        }

        if (countLeft > 0)
            return countLeft;

        return 0;
    }
}
