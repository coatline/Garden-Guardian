using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : InteractableSlot
{

}


//[SerializeField] float selectedScaleMultiplier;
//[SerializeField] GameObject descriptionScreen;
//[SerializeField] TMP_Text descriptionText;
//[SerializeField] TMP_Text itemNameText;
//[SerializeField] Image backgroundImage;
//[SerializeField] Color selectedColor;
//Color initialColor;
//bool selected;

//ItemHolder itemHolder;
//ItemHolder cursor;

//private void Awake()
//{
//    cursor = FindObjectOfType<CursorInventory>().holder;
//    initialColor = backgroundImage.color;
//}

//bool rightMouseDown;

//public void AssignItemHolder(ItemHolder ih)
//{
//    itemHolder = ih;
//}

//public void OnPointerClick(PointerEventData eventData)
//{
//    if (eventData.button == PointerEventData.InputButton.Left)
//    {
//        LeftClickOnSlot();
//    }
//}

//public void OnPointerDown(PointerEventData eventData)
//{
//    if (eventData.button == PointerEventData.InputButton.Right)
//    {
//        rightMouseDown = true;
//        StartCoroutine(OnRightMouse());
//    }
//}

//public void OnPointerUp(PointerEventData eventData)
//{
//    rightMouseDown = false;
//}

//public void OnPointerEnter(PointerEventData eventData)
//{
//    MouseOver();

//}

//public void OnPointerExit(PointerEventData eventData)
//{
//    UnMouseOver();
//}

//void MouseOver()
//{
//    if (!itemHolder.ItemType) return;

//    if (!selected)
//        transform.localScale = Vector3.one * (1 + ((selectedScaleMultiplier - 1) / 2));

//    descriptionScreen.SetActive(true);
//    itemNameText.text = itemHolder.ItemType.Name;
//    descriptionText.text = itemHolder.ItemType.Description;
//}

//public void UnMouseOver()
//{
//    descriptionScreen.SetActive(false);

//    if (!selected)
//        transform.localScale = Vector3.one;
//}

//IEnumerator OnRightMouse()
//{
//    RightClickOnSlot();
//    yield return new WaitForSeconds(.65f);

//    while (rightMouseDown)
//    {
//        if (rightMouseDown)
//        {
//            RightClickOnSlot();
//        }

//        yield return new WaitForSeconds(.04f);
//    }
//}

//void RightClickOnSlot()
//{
//    if (cursor.ItemType)
//    {
//        //if (CurrentItem)
//        {
//            if (cursor.ItemType == itemHolder.ItemType || itemHolder.ItemType == null)
//            {
//                // They are the same
//                // Take one from the cursor
//                TakeItemFromCursor(1);
//            }
//        }
//        //else
//        //{
//        //    // Cursor has something I have nothing
//        //    // Take one from the cursor
//        //    TryTakeCountFromCursor(1);
//        //}
//    }
//    else
//    {
//        if (itemHolder.ItemType)
//        {
//            // I have something Cursor has nothing
//            // Give half to cursor
//            GiveItemToCursor(Mathf.CeilToInt((itemHolder.Count / 2f)));
//        }
//    }
//}

//void LeftClickOnSlot()
//{
//    if (cursor.ItemType)
//    {
//        if (itemHolder.ItemType)
//        {
//            if (cursor.ItemType == itemHolder.ItemType)
//            {
//                // They are the same 
//                // Try to combine them
//                // Give cursor leftovers
//                CombineItemsIntoSlot();
//            }
//            else
//            {
//                SwapItemsWithCursor();
//            }
//        }
//        else
//        {
//            // I have nothing and cursor has something
//            // Take item from cursor
//            TakeItemFromCursor();
//        }
//    }
//    else
//    {
//        if (itemHolder.ItemType)
//        {
//            // Cursor has nothing I have something
//            // Give cursor my item
//            GiveItemToCursor();
//        }
//        else
//        {
//            // Neither of us have anything to give
//            // Do nothing at all
//        }
//    }
//}

//void SwapItemsWithCursor()
//{
//    Item oldItem = cursor.ItemType;
//    int oldCount = cursor.Count;

//    cursor.ItemType = itemHolder.ItemType;
//    cursor.Count += itemHolder.Count;

//    itemHolder.ItemType = oldItem;
//    itemHolder.Count = oldCount;
//}

//void CombineItemsIntoSlot()
//{
//    int overflow = itemHolder.TryAdd(cursor.Count);
//    cursor.Remove(cursor.Count - overflow);
//}

//void TakeItemFromCursor(float count = .1f)
//{
//    // Add to slot
//    itemHolder.ItemType = cursor.ItemType;
//    itemHolder.Count += count == .1f ? cursor.Count : (int)count;

//    // Remove from cursor
//    if (count == .1f)
//    {
//        cursor.ItemType = null;
//    }
//    else
//    {
//        cursor.Count -= (int)count;
//    }
//}

//void GiveItemToCursor(float count = .1f)
//{
//    // Add to cursor
//    cursor.ItemType = itemHolder.ItemType;
//    cursor.Count += count == .1f ? itemHolder.Count : (int)count;

//    // Remove from slot
//    if (count == .1f)
//    {
//        itemHolder.ItemType = null;
//    }
//    else
//    {
//        itemHolder.Count -= (int)count;
//    }
//}

//public void DeSelect()
//{
//    selected = false;
//    transform.localScale = Vector3.one;
//    backgroundImage.color = initialColor;
//}

//public void Select()
//{
//    selected = true;
//    transform.localScale = Vector3.one * selectedScaleMultiplier;
//    backgroundImage.color = selectedColor;
//}

//public ItemHolder GetItem() { return itemHolder; }