using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-3)]
public class InventoryDisplayUnit : MonoBehaviour
{
    [SerializeField] InventorySlot slotPrefab;
    [SerializeField] GameObject grouper;
    [SerializeField] int initialSlots;

    List<InventorySlot> slots = new List<InventorySlot>();
    List<TMP_Text> texts = new List<TMP_Text>();
    List<Image> images = new List<Image>();
    Inventory currentInventory;

    public bool Displaying { get; private set; }

    private void Awake()
    {
        for (int i = 0; i < initialSlots; i++)
        {
            AddSlot();
        }
    }

    public void Display(Inventory i)
    {
        if (Displaying)
        {
            if (i == currentInventory) return;
            else
                StopDisplaying();
        }

        currentInventory = i;

        // Ensure enough slots
        if (i.Size > slots.Count)
        {
            print($"Short {i.Size - slots.Count} slots!");

            for (int k = 0; k < i.Size - slots.Count; k++)
            {
                AddSlot();
            }
        }

        for (int k = 0; k < i.Size; k++)
        {
            ItemContainer itemH = i.GetItem(k);

            slots[k].gameObject.SetActive(true);

            slots[k].AssignItemContainer(itemH);

            itemH.SetVisuals(images[k], texts[k]);
        }

        grouper.SetActive(true);
        Displaying = true;
    }

    public void StopDisplaying()
    {
        grouper.SetActive(false);

        for (int k = 0; k < currentInventory.Size; k++)
        {
            slots[k].UnMouseOver();

            ItemContainer itemH = currentInventory.GetItem(k);
            itemH.RemoveAllVisuals();
        }

        Displaying = false;
    }

    public InventorySlot[] GetSlots()
    {
        return slots.ToArray();
    }

    public bool IsDisplayingInventory(Inventory i) { return Displaying && currentInventory == i; }

    void AddSlot()
    {
        InventorySlot slot = Instantiate(slotPrefab, grouper.transform);

        Image itemImage = slot.transform.Find("ItemImage").GetComponent<Image>();
        TMP_Text countText = slot.transform.Find("ItemCount").GetComponent<TMP_Text>();

        itemImage.enabled = false;

        slot.gameObject.SetActive(false);

        slots.Add(slot);
        texts.Add(countText);
        images.Add(itemImage);
    }
}
