using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantVisuals : StructureVisuals
{
    List<WorldPlant> toGrowSprite;
    List<WorldPlant> toGrow;
    // Use hashset!

    protected override void Start()
    {
        toGrowSprite = new List<WorldPlant>();
        toGrow = new List<WorldPlant>();

        for (int x = 0; x < world.Width; x++)
            for (int y = 0; y < world.Height; y++)
            {
                WorldCell cell = world.GetCell(x, y);

                cell.PlantCreated += StructureCreated;

                if (cell.Plant != null)
                {
                    StructureCreated(cell.Plant);
                }
            }
    }

    void GrewUp(WorldObject plant)
    {
        // It would be cool to have a grow up animation for when they grow (Modify the scale).
        // Animator override controllers

        WorldPlant p = plant as WorldPlant;

        if (p.GetPrefab() == null)
        {
            lock (toGrowSprite)
                toGrowSprite.Add(p);
        }
        else
        {
            lock (toGrow)
                toGrow.Add(p);
        }

    }

    protected override void Update()
    {
        base.Update();
        #region Not Needed
        //if (toDestroy.Count > 0)
        //{
        //    lock (toDestroy)
        //    {
        //        for (int i = 0; i < toDestroy.Count; i++)
        //        {
        //            Item[] items = toSpawn[i].GetItems();

        //            for (int k = 0; k < items.Length; k++)
        //            {
        //                LooseItemSpawner.I.SpawnItem(items[k], 1, new Vector3(toDestroy[i].transform.position.x, toDestroy[i].transform.position.y, 0));
        //            }

        //            Destroy(toDestroy[i].gameObject);
        //        }

        //        toSpawn.Clear();
        //        toDestroy.Clear();
        //    }
        //}

        //if (toCreate.Count > 0)
        //{
        //    lock (toCreate)
        //    {

        //        for (int i = 0; i < toCreate.Count; i++)
        //        {
        //            WorldPlant plant = toCreate[i];

        //            StructureRenderer sr = Instantiate(plant.GetPrefab(), new Vector3(plant.X + .5f, plant.Y + .5f, 0), Quaternion.identity, transform);

        //            sr.SetSprite(plant.GetVisual());

        //            plantRenderers.Add(plant, sr);
        //        }

        //        toCreate.Clear();
        //    }
        //}
        #endregion

        if (toGrow.Count > 0)
        {
            lock (toGrow)
            {
                for (int i = 0; i < toGrow.Count; i++)
                {
                    WorldPlant plant = toGrow[i];

                    if (plant == null) continue;

                    Destroy(structures[plant].gameObject);
                    StructureRenderer sr = Instantiate(plant.GetPrefab(), new Vector3(plant.X + .5f, plant.Y + .5f, 0), Quaternion.identity, transform);
                    sr.SetSprite(plant.GetVisual());

                    structures.Remove(plant);
                    structures.Add(plant, sr);
                }
            }

            toGrow.Clear();
        }

        if (toGrowSprite.Count > 0)
        {
            lock (toGrowSprite)
            {
                for (int i = 0; i < toGrowSprite.Count; i++)
                {
                    if (toGrowSprite[i] != null)
                        StructureVisualChanged(toGrowSprite[i]);
                    else
                        Debug.LogError("Null Structure?");
                }

                toGrowSprite.Clear();
            }

        }
    }

    protected override void StructureCreated(WorldObject wo)
    {
        base.StructureCreated(wo);

        WorldPlant plant = (wo as WorldPlant);
        plant.Destroyed += StructureDestroyed;
        plant.Grew += GrewUp;
    }

    protected override void StructureDestroyed(WorldObject wo)
    {
        base.StructureDestroyed(wo);
        wo.Cell.Plant = null;
    }

    //void PlantCreated(WorldPlant plant)
    //{
    //    plant.OnVisualChanged += PlantVisualChanged;
    //    plant.PlantDestroyed += PlantRemoved;

    //    StructureRenderer sr = Instantiate(plant.GetPrefab(), new Vector3(plant.X + .5f, plant.Y + .5f, 0), Quaternion.identity, transform);

    //    sr.SetSprite(plant.GetVisual());

    //    structures.Add(plant, sr);
    //}

    //void PlantRemoved(WorldPlant plant)
    //{
    //    // Not running on the thread

    //    Item[] items = plant.CurrentProperties.DropPool.GetItems();

    //    StructureRenderer visual = structures[plant];

    //    for (int k = 0; k < items.Length; k++)
    //    {
    //        LooseItemSpawner.I.SpawnItem(items[k], 1, new Vector3(visual.transform.position.x, visual.transform.position.y, 0));
    //    }

    //    Destroy(visual.gameObject);

    //    #region Not Needed
    //    //print("SDf");
    //    //lock (toDestroy)
    //    //{
    //    //    StructureRenderer sr = plantRenderers[plant];

    //    //    if (plantRenderers.ContainsKey(plant)) { lock (plantRenderers) { plantRenderers.Remove(plant); } }

    //    //    if (plant.CurrentProperties.DropPool == null) { Debug.LogWarning(plant.data.Name + " doesn't have a drop pool for the stage: " + plant.Stage); }

    //    //    toSpawn.Add(plant.CurrentProperties.DropPool);
    //    //    toDestroy.Add(sr);
    //    //} 
    //    #endregion
    //}
}
