using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class PlantGrower : MonoBehaviour
{
    List<WorldPlant> growingPlants;
    World world;

    void Start()
    {
        world = WorldController.I.World;

        growingPlants = new List<WorldPlant>();

        for (int x = 0; x < world.Width; x++)
            for (int y = 0; y < world.Height; y++)
            {
                WorldCell cell = world.GetCell(x, y);

                cell.PlantCreated += PlantCreated;

                if (cell.Plant != null)
                {
                    PlantCreated(cell.Plant);
                }
            }
    }

    void PlantCreated(WorldPlant plant)
    {
        growingPlants.Add(plant);
        plant.FinishedGrowing += FinishedGrowing;
        plant.Destroyed += PlantRemoved;
        plant.RegrowPlant += RegrowPlant;
    }

    void RegrowPlant(WorldPlant plant)
    {
        growingPlants.Add(plant);
    }

    void FinishedGrowing(WorldPlant plant)
    {
        growingPlants.Remove(plant);
    }

    void PlantRemoved(WorldObject plant)
    {
        growingPlants.Remove(plant as WorldPlant);
    }

    bool growing;

    void GrowPlants(object a)
    {
        try
        {
            for (int i = 0; i < growingPlants.Count; i++)
            {
                if (growingPlants[i] == null) { continue; }
                growingPlants[i].Grow(currentDt);
            }
        }
        catch (Exception e)
        {
            print(e.Message);
        }

        growing = false;
    }

    float growingDeltaTime;
    float currentDt;

    void Update()
    {
        if (growingPlants.Count == 0) return;

        growingDeltaTime += Time.deltaTime;

        if (!growing)
        {
            growing = true;

            currentDt = growingDeltaTime;
            GrowPlants(this);
            //ThreadPool.QueueUserWorkItem(GrowPlants);

            growingDeltaTime = 0;
        }
    }
}
