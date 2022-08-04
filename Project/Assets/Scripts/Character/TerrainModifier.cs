using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainModifier : MonoBehaviour
{
    //World world;

    //void Start()
    //{
    //    world = WorldController.I.World;
    //}

    //bool TryHitStructureAt(int x, int y, int damage, ToolType tool = ToolType.None)
    //{
    //    WorldCell c = world.GetCell(x, y);

    //    //if (c.Plant != null)
    //    //{
    //    //    if (c.Plant.CurrentProperties.CanHitWith(tool))
    //    //    {
    //    //        c.Plant.TakeDamage(damage);
    //    //        return true;
    //    //    }
    //    //}
    //    //else
    //    if (c.Structure != null)
    //    {
    //        if (c.Structure.data.Properties.CanHitWith(tool))
    //        {
    //            c.Structure.TakeDamage(damage);
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //public void LeftClick(ItemContainer selected, WorldCell cell)
    //{
    //    if (selected.ItemType == null) return;

    //    if (selected.ItemType.ToolType != ToolType.None)
    //    {
    //        UseTool(cell.X, cell.Y, selected.ItemType);
    //    }
    //    else if (selected.ItemType.Structure)
    //    {
    //        Plant p = selected.ItemType.Structure as Plant;

    //        if (cell.Plant != null) return;

    //        // Is it a plant?
    //        if (p != null)
    //        {
    //            // If there is no structure or the structure is a pot or something. 
    //            if (cell.Structure == null || (cell.Structure != null && cell.Structure.data.CanPlantOn))
    //            {
    //                // If the ground is suitable for planting.
    //                if (cell.Ground.Tilled == true || !p.RequiresTilledSoil)
    //                {
    //                    cell.Plant = new WorldPlant(selected.ItemType.Structure as Plant, 0, cell);
    //                }
    //                else
    //                    return;
    //            }
    //            else
    //                return;
    //        }
    //        // Is there already a structure here?
    //        else if (cell.Structure == null)
    //            cell.Structure = new WorldObject(selected.ItemType.Structure, cell);
    //        else
    //            return;

    //        selected.Count--;
    //    }
    //}

    //public void UseTool(int x, int y, Item item)
    //{
    //    WorldCell cell = world.GetCell(x, y);

    //    switch (item.ToolType)
    //    {
    //        //case ToolType.Pickaxe: case ToolType.Axe: TryHitStructureAt(x, y, item.ToolPower, item.ToolType); break;

    //        case ToolType.Hoe:

    //            //if (cell.Structure != null || cell.Plant != null)
    //            //{
    //            //    if (TryHitStructureAt(x, y, item.ToolPower, item.ToolType))
    //            //    {
    //            //        break;
    //            //    }
    //            //}
    //            //else
    //            {
    //                TillTileAt(x, y);
    //            }

    //            break;
    //    }
    //}

    //void TillTileAt(int x, int y)
    //{
    //    WorldCell c = world.GetCell(x, y);

    //    c.Ground = DataLibrary.I.grounds["Tilled Soil"] as Ground;
    //}
}
