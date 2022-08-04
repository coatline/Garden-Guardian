using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryType
{
    Hotbar,
    Backpack,
    Chest
}

[DefaultExecutionOrder(-2)]
public class InventoryDisplayer : MonoBehaviour
{
    [SerializeField] InventoryDisplayUnit[] displays;

    Inventory pI;
    Inventory playerBackpack
    {
        get
        {
            if (pI == null) { pI = FindObjectOfType<Character>().Backpack; }

            return pI;
        }
        set
        {
            pI = value;
        }
    }

    #region Statics
    static InventoryDisplayer instance;
    public static InventoryDisplayer I
    {
        get
        {
            return instance;
        }
        set
        {
            if (instance) { return; }
            else
            {
                instance = value;
            }
        }
    }
    #endregion

    private void Awake()
    {
        #region Statics
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning($"Already a {name} in scene. Deleting this one!");
            Destroy(gameObject);
            return;
        }
        #endregion
    }

    public void DisplayBackpack()
    {
        GetDisplay(InventoryType.Backpack).Display(playerBackpack);
    }

    void StopDisplayingBackpack()
    {
        GetDisplay(InventoryType.Backpack).StopDisplaying();
    }

    public void Display(Inventory i, InventoryType it)
    {
        if (it != InventoryType.Hotbar)
            DisplayBackpack();

        GetDisplay(it).Display(i);
    }

    public void StopDisplaying(InventoryType it)
    {
        if (it == InventoryType.Backpack)
            StopDisplayingBackpack();
        else
            GetDisplay(it).StopDisplaying();
    }

    public bool IsDisplaying(InventoryType it)
    {
        return GetDisplay(it).Displaying;
    }

    public bool IsDisplaying(InventoryType it, Inventory i)
    {
        return GetDisplay(it).IsDisplayingInventory(i);
    }

    public InventoryDisplayUnit GetDisplay(InventoryType it) { return displays[(int)it]; }
}
