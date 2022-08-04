using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldCell
{
    public int X { get; private set; }
    public int Y { get; private set; }

    #region Ground
    Ground ground;

    public Ground Ground
    {
        get { return ground; }
        set
        {
            if (value == ground) return;

            ground = value;

            GroundChanged?.Invoke(this);
        }
    }

    Action<WorldCell> GroundChanged;

    public void RegisterOnGroundChanged(Action<WorldCell> a)
    {
        GroundChanged += a;
    }

    #endregion

    #region Structure
    WorldObject structure;

    public WorldObject Structure
    {
        get { return structure; }
        set
        {
            if (value == structure) return;

            structure = value;

            if (value != null)
                StructureCreated?.Invoke(structure);
        }
    }

    public event Action<WorldObject> StructureCreated;

    #endregion

    #region Plant
    WorldPlant p;

    public WorldPlant Plant
    {
        get
        {
            return p;
        }
        set
        {
            if (value == p) return;

            //// If I am setting it to null and there is already a plant here to remove.
            //if (value == null && p != null)
            //{
            //    PlantRemoved?.Invoke(p);
            //}

            p = value;

            // If I am setting it to a new plant.
            if (value != null)
            {
                PlantCreated?.Invoke(p);

                //p.Destroyed += ((p) => { p = null; });
                //p.RegisterDestroyed((p) => { Plant = null; });
            }
        }
    }

    public event Action<WorldPlant> PlantCreated;

    #endregion

    public WorldCell(Ground type, int x, int y)
    {
        this.Ground = type;
        this.X = x;
        this.Y = y;
    }
}
