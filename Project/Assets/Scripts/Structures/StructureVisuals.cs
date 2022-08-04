using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StructureVisuals : MonoBehaviour
{
    protected Dictionary<WorldObject, StructureRenderer> structures;

    protected World world;

    private void Awake()
    {
        structures = new Dictionary<WorldObject, StructureRenderer>();
        toPlay = new List<StructureRenderer>();
        animations = new List<string>();
        world = WorldController.I.World;
    }

    protected virtual void Start()
    {
        for (int x = 0; x < world.Width; x++)
            for (int y = 0; y < world.Height; y++)
            {
                WorldCell cell = world.GetCell(x, y);
                cell.StructureCreated += StructureCreated;

                if (cell.Structure != null)
                {
                    StructureCreated(cell.Structure);
                }
            }
    }

    protected virtual void StructureDestroyed(WorldObject wo)
    {
        // Spawn in items
        LooseItemSpawner.I.SpawnItems(wo.CurrentProperties.DropPool, new Vector3(wo.X, wo.Y, 0));
        print(wo.CurrentProperties.DropPool.GetItems().Length);

        wo.Cell.Structure = null;


        // Destroy Visual
        if (structures.TryGetValue(wo, out StructureRenderer sr))
        {
            // Play an animation here.
            // Remove the reference to the visual
            print("GTTT SDFSDFSFSDF?" + name);
            structures.Remove(wo);
            Destroy(sr.RootGob);
        }
        else
        {
            print("No structure?" + name);
        }
    }

    protected void StructureVisualChanged(WorldObject wo)
    {
        structures[wo].SetSprite(wo.GetVisual());
    }

    List<string> animations;
    List<StructureRenderer> toPlay;

    void StructurePlayAnimation(WorldObject wo, string anim)
    {
        lock (toPlay)
        {
            toPlay.Add(structures[wo]);
            animations.Add(anim);
        }
    }

    protected virtual void Update()
    {
        if (toPlay.Count > 0)
        {
            lock (toPlay)
            {
                for (int i = 0; i < toPlay.Count; i++)
                {
                    toPlay[i].PlayAnimation(animations[i]);
                }

                toPlay.Clear();
                animations.Clear();
            }
        }
    }

    protected virtual void StructureCreated(WorldObject wo)
    {
        StructureRenderer sr = Instantiate(wo.GetPrefab(), new Vector3(wo.X + .5f, wo.Y + .5f, 0), Quaternion.identity, transform);

        wo.Destroyed += StructureDestroyed;
        wo.PlayAnimation += StructurePlayAnimation;
        wo.OnVisualChanged += StructureVisualChanged;

        if (!structures.ContainsKey(wo))
        {
            structures.Add(wo, sr);
        }
    }
}
