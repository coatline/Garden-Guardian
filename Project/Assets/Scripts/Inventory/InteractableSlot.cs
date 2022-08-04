using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableSlot : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected float selectedScaleMultiplier;
    [SerializeField] Image backgroundImage;
    [SerializeField] Color selectedColor;
    protected bool selected;
    Color initialColor;

    protected ItemContainer itemContainer;
    protected ItemContainer cursor;
    CursorInventory cursori;

    private void Awake()
    {
        cursori = FindObjectOfType<CursorInventory>();

        if (backgroundImage != null)
            initialColor = backgroundImage.color;
    }

    void Start()
    {
        cursor = cursori.holder;
    }

    bool rightMouseDown;

    public void AssignItemContainer(ItemContainer ih)
    {
        itemContainer = ih;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            LeftClickOnSlot();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            rightMouseDown = true;
            StartCoroutine(OnRightMouse());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rightMouseDown = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseOver();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnMouseOver();
    }

    void MouseOver()
    {
        cursori.MouseOver(itemContainer);

        if (!selected)
            transform.localScale = Vector3.one * (1 + ((selectedScaleMultiplier - 1) / 2));
    }

    public void UnMouseOver()
    {
        cursori.UnMouseOver(itemContainer);

        if (!selected)
            transform.localScale = Vector3.one;
    }

    IEnumerator OnRightMouse()
    {
        RightClickOnSlot();
        yield return new WaitForSeconds(.65f);

        while (rightMouseDown)
        {
            if (rightMouseDown)
            {
                RightClickOnSlot();
            }

            yield return new WaitForSeconds(.04f);
        }
    }

    protected virtual void RightClickOnSlot()
    {
        if (cursor == null)
        {
            Debug.LogWarning("Cursor is null");
        }

        if (cursor.ItemType)
        {
            //if (CurrentItem)
            {
                if (cursor.ItemType == itemContainer.ItemType || itemContainer.ItemType == null)
                {
                    // They are the same
                    // Take one from the cursor
                    TakeItemFromCursor(1);
                }
            }
            //else
            //{
            //    // Cursor has something I have nothing
            //    // Take one from the cursor
            //    TryTakeCountFromCursor(1);
            //}
        }
        else
        {
            if (itemContainer.ItemType)
            {
                // I have something Cursor has nothing
                // Give half to cursor
                GiveItemToCursor(Mathf.CeilToInt((itemContainer.Count / 2f)));
            }
        }
    }

    protected virtual void LeftClickOnSlot()
    {
        if (cursor == null)
        {
            Debug.LogError("Cursor is null");
        }

        if (cursor.ItemType)
        {
            if (itemContainer.ItemType)
            {
                if (cursor.ItemType == itemContainer.ItemType)
                {
                    // They are the same 
                    // Try to combine them
                    // Give cursor leftovers
                    CombineItemsIntoSlot();
                }
                else
                {
                    SwapItemsWithCursor();
                }
            }
            else
            {
                // I have nothing and cursor has something
                // Take item from cursor
                TakeItemFromCursor();
            }
        }
        else
        {
            if (itemContainer.ItemType)
            {
                // Cursor has nothing I have something
                // Give cursor my item
                GiveItemToCursor();
            }
            else
            {
                // Neither of us have anything to give
                // Do nothing at all
            }
        }
    }

    protected void SwapItemsWithCursor()
    {
        Item oldItem = cursor.ItemType;
        int oldCount = cursor.Count;

        cursor.ItemType = itemContainer.ItemType;
        cursor.Count += itemContainer.Count;

        itemContainer.ItemType = oldItem;
        itemContainer.Count = oldCount;
    }

    protected void CombineItemsIntoSlot()
    {
        int overflow = itemContainer.TryAdd(cursor.Count);
        cursor.Remove(cursor.Count - overflow);
    }

    protected void TakeItemFromCursor(float count = .1f)
    {
        // Add to slot
        itemContainer.ItemType = cursor.ItemType;
        itemContainer.Count += count == .1f ? cursor.Count : (int)count;

        // Remove from cursor
        if (count == .1f)
        {
            cursor.ItemType = null;
        }
        else
        {
            cursor.Count -= (int)count;
        }
    }

    protected void GiveItemToCursor(float count = .1f)
    {
        // Add to cursor
        cursor.ItemType = itemContainer.ItemType;
        cursor.Count += count == .1f ? itemContainer.Count : (int)count;

        // Remove from slot
        if (count == .1f)
        {
            itemContainer.ItemType = null;
        }
        else
        {
            itemContainer.Count -= (int)count;
        }
    }

    public virtual void DeSelect()
    {
        selected = false;
        transform.localScale = Vector3.one;

        if (backgroundImage != null)
            backgroundImage.color = initialColor;
    }

    public virtual void Select()
    {
        selected = true;
        transform.localScale = Vector3.one * selectedScaleMultiplier;

        if (backgroundImage != null)
            backgroundImage.color = selectedColor;
    }

    public ItemContainer GetItem() { return itemContainer; }
}
