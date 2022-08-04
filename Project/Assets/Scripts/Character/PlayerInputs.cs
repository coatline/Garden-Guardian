using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] HotbarController hotbarController;
    [SerializeField] RecoilAnimation recoil;
    [SerializeField] ItemHolder itemHolder;
    [SerializeField] Inspector inspector;
    [SerializeField] TerrainModifier tm;
    [SerializeField] Animator animator;
    [SerializeField] ItemUser itemUser;
    CursorInventory cursorInventory;
    CameraFollowWithBarriers cfwb;
    [SerializeField] Character c;
    [SerializeField] int reach;
    WorldCell cellOver;
    World world;

    bool running;

    private void Start()
    {
        world = WorldController.I.World;

        cursorInventory = FindObjectOfType<CursorInventory>();
        cfwb = FindObjectOfType<CameraFollowWithBarriers>();
    }

    void Update()
    {
        if (cfwb.enabled == false) { c.Move(Vector3.zero); return; }

        DoMouseWheel();
        DoMovement();

        Vector3 mousePos = GetMousePos();

        itemHolder.Aim(mousePos, Vector2.zero);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (InventoryDisplayer.I.IsDisplaying(InventoryType.Backpack))
            {
                InventoryDisplayer.I.StopDisplaying(InventoryType.Backpack);

                if (InventoryDisplayer.I.IsDisplaying(InventoryType.Chest))
                    c.CloseChest();
            }
            else
            {
                InventoryDisplayer.I.DisplayBackpack();
            }
        }

        DoDebug();

        if (MouseOverUi())
            return;


        //if (Vector3.Distance(transform.position, mousePos) > reach)
        //    return;

        if (mousePos.x < 0 || mousePos.x > world.Width - 1 || mousePos.y < 0 || mousePos.y > world.Height - 1) return;
        WorldCell cell = world.GetCell((int)mousePos.x, (int)mousePos.y);


        if (cellOver != cell)
        {
            cellOver = cell;
            inspector.MouseOverCell(cell);
        }

        bool leftClicked = Input.GetMouseButton(0);
        bool rightClicked = Input.GetMouseButtonDown(1);

        if (rightClicked || leftClicked)
        {
            ItemContainer selected = CurrentItem();

            if (leftClicked)
            {
                if (selected.ItemType)
                    itemUser.TryUseItem(cell);
                //tm.LeftClick(selected, cell);
            }
            else
            {
                if (cell.Plant != null)
                {
                    if (cell.Plant.Harvestable)
                    {
                        recoil.Recoil(new RecoilAnimation.RecoilSettings(-.2f, 5, .1f));
                        cell.Plant.Harvest();
                    }
                }

                //if (cell.Structure != null)
                //{
                //    if (cell.Structure.inventory != null)
                //    {
                //        Inventory chest = cell.Structure.inventory;

                //        if (InventoryDisplayer.I.IsDisplaying(InventoryType.Chest, chest))
                //            c.CloseChest();
                //        else
                //            c.OpenChest(chest);

                //        return;
                //    }
                //}
            }
        }
    }

    ItemContainer CurrentItem()
    {
        return cursorInventory.holder.ItemType == null ? hotbarController.GetSelected() : cursorInventory.holder;
    }

    bool MouseOverUi()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    Vector3 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
    }

    void DoDebug()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            c.Inventory.TryAddItem(DataLibrary.I.Items["Chest"] as Item, 1);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            c.Inventory.TryAddItem(DataLibrary.I.Items["Stone"] as Item, 1);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKey(KeyCode.F))
        {
            Structure stone = DataLibrary.I.Structures["Stone"] as Structure;
            LooseItemSpawner.I.SpawnItems(stone.Properties.DropPool, GetMousePos());
        }
    }

    void DoMovement()
    {
        Vector2 inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        c.Move(inputs);

        //if (inputs.magnitude > 0)
        //{
        //    if (!running)
        //    {
        //        running = true;
        //        animator.SetBool("Running", true);
        //    }
        //}
        //else
        //{
        //    if (running)
        //    {
        //        running = false;
        //        animator.SetBool("Running", false);
        //    }
        //}
    }

    void DoMouseWheel()
    {
        float mouseWheel = Input.GetAxisRaw("Mouse ScrollWheel");

        if (mouseWheel != 0)
            hotbarController.Scroll(-mouseWheel);
    }
}
