using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Character : Entity
{
    [SerializeField] int hotbarSlots, backpackSlots;
    [SerializeField] int initialMoney;

    [SerializeField] HotbarController hotbarController;
    [SerializeField] List<ItemPackage> startingItems;
    [SerializeField] Animator healthBarAnimator;
    [SerializeField] Image healthBarImage;
    [SerializeField] ItemHolder itemHolder;
    [SerializeField] GameObject pauseMenu;

    public InventoryHolder Inventory { get; private set; }
    public Inventory Backpack { get; private set; }

    Inventory hotbar;

    protected override void Awake()
    {
        base.Awake();

        Backpack = new Inventory(backpackSlots) { Name = "Backack" };
        Inventory = new InventoryHolder() { Name = "Inventory" };
        hotbar = new Inventory(hotbarSlots) { Name = "Hotbar" };

        Inventory.AddInventory(hotbar);
        Inventory.AddInventory(Backpack);

        for (int i = 0; i < startingItems.Count; i++)
            Inventory.TryAddItem(startingItems[i].Item, startingItems[i].Count);

        InventoryDisplayer.I.Display(hotbar, InventoryType.Hotbar);

        hotbarController.ItemSwitched += ChangedItem;
    }

    public override void Damage(int val, Vector2 knockBack)
    {
        base.Damage(val, knockBack);
        healthBarAnimator.Play("Subtract");
        healthBarImage.fillAmount = (float)health / (float)maxHealth;
    }

    void ChangedItem(ItemContainer i)
    {
        itemHolder.ChangeItem(i);
    }

    public override void Die()
    {
        pauseMenu.SetActive(true);
        pauseMenu.transform.GetChild(1).GetComponent<TMP_Text>().text = $"You harvested {9999 - Inventory.Contains(DataLibrary.I.Items["Watermelon"] as Item, 9999)} watermelons.";
    }

    public void Move(Vector3 inputs)
    {
        if (inputs.magnitude == 0)
            StopMoving();
        else
            MoveTowardsPos(inputs, true);
        //rb.velocity = inputs.normalized * initialSpeed;

        //if (inputs.x > 0)
        //{
        //    sr.flipX = false;
        //}
        //else if (inputs.x < 0)
        //{
        //    sr.flipX = true;
        //}
    }

    public int PickupLooseItem(ItemContainer item)
    {
        return Inventory.TryAddItem(item.ItemType, item.Count);
    }

    Inventory currentChest;

    public void OpenChest(Inventory chest)
    {
        currentChest = chest;

        Inventory.AddInventory(chest);

        InventoryDisplayer.I.Display(chest, InventoryType.Chest);
    }

    public void CloseChest()
    {
        Inventory.RemoveInventory(currentChest);

        currentChest = null;

        InventoryDisplayer.I.StopDisplaying(InventoryType.Chest);
    }
}
