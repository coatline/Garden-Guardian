using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainer
{
    event Action<ItemContainer> typeChanged;

    Item i;
    public Item ItemType
    {
        get
        {
            return i;
        }
        set
        {
            if (i == value) return;

            i = value;

            if (value == null)
            {
                Count = 0;
            }


            typeChanged?.Invoke(this);

            UpdateImage();
        }
    }

    event Action<ItemContainer> countChanged;

    int c;
    public int Count
    {
        get { return c; }
        set
        {
            if (c == value) return;

            c = value;

            if (ItemType != null)
            {
                if (c > ItemType.MaxStack)
                {
                    c = ItemType.MaxStack;
                }
                else if (c <= 0)
                {
                    ItemType = null;
                    c = 0;
                }
            }

            countChanged?.Invoke(this);

            UpdateCountText();
        }
    }

    public bool Interactable { get; set; }
    TMP_Text countText;
    SpriteRenderer sr;
    Image itemImage;

    public int SpaceLeft()
    {
        return ItemType.MaxStack - Count;
    }

    /// <returns>Remainder count</returns>
    public int TryAdd(int count)
    {
        int initialCount = Count;
        Count += count;
        return Mathf.Max((count + initialCount) - Count, 0);
    }

    public void RemoveAllVisuals()
    {
        itemImage = null;
        sr = null;
        countText = null;
    }

    public void SetVisuals(Image i, TMP_Text t)
    {
        itemImage = i;
        countText = t;

        UpdateVisuals();
    }

    public void SetSprite(SpriteRenderer sr)
    {
        this.sr = sr;

        UpdateImage();
    }


    public void RemoveAll()
    {
        ItemType = null;
    }

    public void Remove(int amount)
    {
        Count -= amount;
    }

    void UpdateVisuals()
    {
        UpdateImage();
        UpdateCountText();
    }

    void UpdateCountText()
    {
        if (countText != null)
        {
            if (ItemType && ItemType.MaxStack > 1)
                countText.text = Count.ToString();
            else
                countText.text = "";
        }
    }

    void UpdateImage()
    {
        if (ItemType)
        {
            if (itemImage != null)
            {
                itemImage.enabled = true;
                itemImage.sprite = ItemType.Sprite;
            }
            if (sr != null)
                this.sr.sprite = ItemType.Sprite;
        }
        else
        {
            if (itemImage != null)
                itemImage.enabled = false;
            if (sr != null)
                sr.sprite = null;
        }
    }

    public ItemContainer(Item itemType, int count, Image i = null, SpriteRenderer sr = null, TMP_Text t = null)
    {
        ItemType = itemType;
        Count = count;

        this.sr = sr;
        this.itemImage = i;
        this.countText = t;

        UpdateVisuals();
    }

    public void RegisterOnCountChanged(Action<ItemContainer> cb)
    {
        countChanged += cb;
    }

    public void RegisterOnItemChanged(Action<ItemContainer> cb)
    {
        typeChanged += cb;
    }
}
