using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inspector : MonoBehaviour
{
    [SerializeField] GameObject mouseTileVisuals;
    [SerializeField] TMP_Text mouseTileText;

    WorldCell mouseOverCell;

    public void MouseOverCell(WorldCell cell)
    {
        mouseTileVisuals.SetActive(true);
        this.mouseOverCell = cell;
    }

    public void DisableCurrentTile()
    {
        mouseTileVisuals.SetActive(false);
        mouseOverCell = null;
    }

    void Update()
    {
        if (mouseOverCell != null)
        {
            InspectMouseTile();
        }
    }

    //void InspectGround()
    //{
    //    inspectNameText.text = $"{inspectCell.Ground.name}";
    //    inspectText.text = $"Fertility: {inspectCell.Ground.Fertility * 100}%";
    //    inspectText.text = $"{inspectCell.Ground.Description}";
    //}

    //void InspectPlant()
    //{
    //    WorldPlant plant = inspectCell.Plant;

    //    if (plant == null) { DisableInspection(); return; }

    //    //lock (plant)
    //    {
    //        inspectNameText.text = $"{plant.data.name}";
    //        inspectText.text = $"{Mathf.CeilToInt(plant.hitPoints)} / {plant.CurrentProperties.HitPoints}\n";

    //        if (plant.Mature)
    //        {
    //            inspectText.text += $"Mature.\n";

    //            if (plant.Harvestable)
    //                inspectText.text += $"Harvestable.\n";
    //            else if (plant.Plant.Reproduces)
    //            {
    //                inspectText.text += $"Produce: {Mathf.Clamp((plant.ToProduce.GrowthPercentage * 100), 0, 100f).ToString("0.00")}%\n" +
    //                $"Produce Stage: {plant.ToProduce.Stage + 1}/{plant.Plant.MatureProperties.Count}\n" +
    //                $"Growth Rate: {plant.GrowthRate() * 100}%\n";
    //            }
    //        }
    //        else
    //        {
    //            inspectText.text += $"Maturity: {Mathf.Clamp((plant.ToMature.GrowthPercentage * 100), 0, 100f).ToString("0.00")}%\n" +
    //            $"Growth Stage: {plant.ToMature.Stage + 1}/{plant.Plant.GrowthProperties.Count}\n" +
    //            $"Growth Rate: {plant.GrowthRate() * 100}%\n";
    //        }
    //    }
    //}

    //void InspectStructure()
    //{
    //    WorldObject structure = inspectCell.Structure;
    //    if (structure == null) { DisableInspection(); return; }

    //    inspectNameText.text = $"{structure.data.name}";
    //    inspectText.text = $"{Mathf.CeilToInt(structure.hitPoints)} / {structure.CurrentProperties.HitPoints}";
    //}

    void InspectMouseTile()
    {
        mouseTileText.text = "";
        //if (mouseOverCell.Fire != null)
        //{
        //    lock (mouseOverCell.Fire)
        //    {
        //        mouseTileText.text += $"Burning ({(mouseOverCell.Fire.intensity * 100).ToString("0.00")}%)\n";
        //        //mouseTileText.text += $"({(mouseOverCell.Fire.intensity * 100).ToString("0.00")}%)\n";
        //    }
        //}

        if (mouseOverCell.Plant != null)
        {
            WorldPlant p = mouseOverCell.Plant;

            if (p.Mature)
            {
                if (!p.Harvestable)
                    mouseTileText.text += $"{Mathf.Clamp((p.ToProduce.GrowthPercentage * 100), 0, 100f).ToString("0.00")}% HP: {p.HitPoints}/{p.CurrentProperties.HitPoints}";
                else
                    mouseTileText.text += $"HP: {p.HitPoints}/{p.CurrentProperties.HitPoints} Harvestable";
            }
            else
                mouseTileText.text += $"{Mathf.Clamp((p.ToMature.GrowthPercentage * 100), 0, 100f).ToString("0.00")}% HP: {p.HitPoints}/{p.CurrentProperties.HitPoints}";

            mouseTileText.text += $" {mouseOverCell.Plant.Data.name}\n";
        }

        if (mouseOverCell.Structure != null)
        {
            mouseTileText.text += $"{mouseOverCell.Structure.Data.name}\n";
        }

        mouseTileText.text += $"{mouseOverCell.Ground.name}\n";

        mouseTileText.text += $"{mouseOverCell.X}, {mouseOverCell.Y}\n";
    }
}
