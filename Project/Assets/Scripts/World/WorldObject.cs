using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldObject
{
    public int X { get; protected set; }
    public int Y { get; protected set; }

    public virtual StructureProperties CurrentProperties { get { return Data.Properties; } }
    public Inventory Inventory { get; protected set; }
    public WorldCell Cell { get; protected set; }
    public Structure Data { get; protected set; }
    public float HitPoints { get; protected set; }
    public bool Null { get; protected set; }

    // This fires when say a wall gets damaged, it needs to appear that way
    public event Action<WorldObject, string> PlayAnimation;
    public event Action<WorldObject> OnVisualChanged;
    public event Action<WorldObject> Destroyed;

    public void TakeDamage(float points)
    {
        if (Null) return;

        HitPoints -= points;

        if (HitPoints <= 0)
        {
            Null = true;
            Destroyed?.Invoke(this);
        }
        else
        {
            PlayAnimation?.Invoke(this, "Damage");
            OnVisualChanged?.Invoke(this);
        }
    }

    protected void Destroy()
    {
        if (Null) return;
        HitPoints = 0;
        Destroyed?.Invoke(this);
    }

    protected void PlayAnim(string anim)
    {
        PlayAnimation?.Invoke(this, anim);
    }

    public Sprite GetVisual()
    {
        // Percentage of health remaining
        float percentage = (float)HitPoints / (float)CurrentProperties.HitPoints;

        //Debug.Log(percentage + " " + (CurrentProperties.DamagedSprites.Length - 1) + " " + (Mathf.CeilToInt((percentage * (CurrentProperties.DamagedSprites.Length))) - 1));

        return CurrentProperties.DamagedSprites[Mathf.CeilToInt((percentage * (CurrentProperties.DamagedSprites.Length))) - 1];
    }

    public StructureRenderer GetPrefab()
    {
        return CurrentProperties.Prefab;
    }

    public WorldObject(Structure s, WorldCell c)
    {
        this.Data = s;
        this.Cell = c;

        this.X = c.X;
        this.Y = c.Y;

        HitPoints = s.Properties.HitPoints;

        if (s.HasInventory)
            Inventory = new Inventory(s.SlotCount);
    }
}


