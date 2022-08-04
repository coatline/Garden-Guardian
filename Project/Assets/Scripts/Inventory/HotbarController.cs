using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class HotbarController : MonoBehaviour
{
    public event System.Action<ItemContainer> ItemSwitched;

    [SerializeField] int hotbarSlots;
    InventorySlot selectedSlot;
    InventorySlot[] slots;
    int index;

    private void Awake()
    {
        slots = new InventorySlot[hotbarSlots];

        AssignHotbarSlots();
        ChangeSelected();
    }

    private void Start()
    {
        ItemSwitched?.Invoke(selectedSlot.GetItem());
    }

    public ItemContainer GetSelected() { return selectedSlot.GetItem(); }

    public void Scroll(float delta)
    {
        if (delta > 0)
        {
            index++;

            if (index >= slots.Length)
                index = 0;
        }
        else
        {
            index--;

            if (index < 0)
                index = slots.Length - 1;
        }

        ChangeSelected();
    }

    void ChangeSelected()
    {
        selectedSlot?.DeSelect();
        selectedSlot = slots[index];
        selectedSlot.Select();
        ItemSwitched?.Invoke(selectedSlot.GetItem());
    }

    void AssignHotbarSlots()
    {
        InventoryDisplayer.I.Display(new Inventory(hotbarSlots), InventoryType.Hotbar);
        slots = InventoryDisplayer.I.GetDisplay(InventoryType.Hotbar).GetSlots();
    }
}
























//public class HotbarController : MonoBehaviour
//{
//    [SerializeField] int hotbarSlots;
//    List<ItemHolder> hotbarItems;
//    InventorySlot selectedSlot;
//    InventorySlot[] slots;
//    int index;

//    public ItemHolder GetSelected()
//    {
//        return items[index];
//    }

//    public HotbarController(int size, int hotbarSlots) : base(size, false)
//    {
//        this.hotbarSlots = hotbarSlots;
//        slots = new InventorySlot[hotbarSlots];

//        AssignHotbarSlots();
//        InitializeItemHolders();
//    }

//    public void Scroll(float delta)
//    {
//        if (delta > 0)
//        {
//            index++;

//            if (index >= slots.Length)
//                index = 0;
//        }
//        else
//        {
//            index--;

//            if (index < 0)
//                index = slots.Length - 1;
//        }

//        ChangeSelected();
//    }

//    void ChangeSelected()
//    {
//        selectedSlot.DeSelect();
//        selectedSlot = slots[index];
//        selectedSlot.Select();
//    }

//    public override ItemHolder GetItem(int index)
//    {
//        return items[index + hotbarSlots];
//    }

//    void AssignHotbarSlots()
//    {
//        InventoryDisplayer.I.Display(InventoryType.Hotbar);

//        //if (hotbarSlots > 0)
//        //{
//        //    slots = InventoryDisplayer.I.CreateHotbar(hotbarSlots);

//        //    selectedSlot = slots[0];

//        //    for (int i = 0; i < hotbarSlots; i++)
//        //    {
//        //        InventorySlot slot = slots[i];
//        //        Image itemImage = slot.transform.Find("ItemImage").GetComponent<Image>();
//        //        TMP_Text countText = slot.transform.Find("ItemCount").GetComponent<TMP_Text>();

//        //        ItemHolder ih = new ItemHolder(null, 0, itemImage, null, countText);
//        //        items.Add(ih);
//        //        slot.AssignItemHolder(ih);
//        //    }

//        //    ChangeSelected();
//        //}
//    }
//}

