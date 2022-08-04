using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldPlant : WorldObject
{
    public override StructureProperties CurrentProperties
    {
        get
        {
            if (Mature)
            {
                return ToProduce.CurrentProperties;
            }
            else
            {
                return ToMature.CurrentProperties;
            }
        }
    }

    public StructureRenderer PrevPrefab { get; private set; }

    public bool Harvestable { get; protected set; }
    public bool Mature { get; protected set; }

    //float reproduceInterval;
    //float reproduceElapsed;
    //float growthInterval;
    //float growthElapsed;
    int maxHitPoints;
    float fertility;

    public event Action<WorldPlant> FinishedGrowing;
    public event Action<WorldPlant> RegrowPlant;
    public event Action<WorldPlant> Grew;
    public Grower ToProduce { get; private set; }
    public Grower ToMature { get; private set; }

    public Plant Plant { get; private set; }

    public WorldPlant(Plant plant, float initialGrowthPercentage, WorldCell cell) : base(plant, cell)
    {
        this.Plant = plant;

        // Set the fertility
        fertility = cell.Ground.Fertility;

        ToMature = new Grower(plant.GrowthProperties, initialGrowthPercentage, false, plant.GrowTime);
        ToProduce = new Grower(plant.MatureProperties, 0, true, plant.ProduceTime);

        ToMature.Grew += StageUp;
        ToMature.Completed += CompletedMaturing;

        ToProduce.Grew += StageUp;
        ToProduce.Completed += CompletedProducing;

        HitPoints = CurrentProperties.HitPoints;
        maxHitPoints = CurrentProperties.HitPoints;
    }

    void CompletedMaturing(Grower g)
    {
        Mature = true;
        Grew?.Invoke(this);

        HitPoints += CurrentProperties.HitPoints - maxHitPoints;
        maxHitPoints = CurrentProperties.HitPoints;

        PlayAnim("Plant_Grow");
    }

    void CompletedProducing(Grower g)
    {
        Harvestable = true;

        Grew?.Invoke(this);
        FinishedGrowing?.Invoke(this);

        PlayAnim("Plant_Grow");
    }

    void StageUp(Grower g)
    {
        HitPoints += CurrentProperties.HitPoints - maxHitPoints;
        maxHitPoints = CurrentProperties.HitPoints;
        //Debug.Log(HitPoints + " " + CurrentProperties.HitPoints + " " + maxHitPoints);
        Grew?.Invoke(this);

        PlayAnim("Plant_Grow");
    }

    public void Grow(float deltaTime)
    {
        float growth = deltaTime * GrowthRate();

        if (!Mature)
        {
            ToMature.Grow(growth);
        }
        else if (!Harvestable)
        {
            ToProduce.Grow(growth);
        }
    }

    public void Harvest()
    {
        LooseItemSpawner.I.SpawnItems(CurrentProperties.DropPool, new Vector3(X, Y, 0));

        ToProduce.Reset();
        HitPoints = CurrentProperties.HitPoints;

        Harvestable = false;

        if (Plant.Reproduces)
        {
            RegrowPlant?.Invoke(this);
            Grew?.Invoke(this);
            PlayAnim("Plant_Grow");
        }
        else
        {
            Destroy();
        }
    }

    /// <returns>The percentage of how fast it is growing in relation to how fast it can grow.</returns>
    public float GrowthRate()
    {
        return fertility;
    }
}

public class Grower
{
    public StructureProperties CurrentProperties
    {
        get
        {
            return properties[Stage];
        }
    }

    public float GrowthPercentage { get { return growthElapsed / goalTime; } }
    public int Stage { get; private set; }

    public event Action<Grower> Completed;
    List<StructureProperties> properties;
    public event Action<Grower> Grew;

    float growthInterval;
    public float growthElapsed;
    public float goalTime;

    public void Reset()
    {
        growthElapsed = 0;
        Stage = 0;
    }

    public Grower(List<StructureProperties> properties, float initialGrowthPercentage, bool finalStageWhenMature, float goalTime)
    {
        this.properties = properties;
        this.goalTime = goalTime;

        // Define interval for each stage
        if (finalStageWhenMature)
            growthInterval = 1f / (Mathf.Max((properties.Count - 1), 1));
        else
            growthInterval = 1f / (properties.Count);

        //Debug.Log(growthInterval + finalStageWhenMature.ToString());

        // Check for initial growth
        if (initialGrowthPercentage > 0)
        {
            growthElapsed = initialGrowthPercentage * goalTime;

            //Debug.Log($"initialGrowthPercentage: {initialGrowthPercentage}, GrowthPercentage: {GrowthPercentage},  changeVisualInterval: {changeVisualInterval}, Stage: {GrowthPercentage / changeVisualInterval}");

            Stage = Mathf.Min(Mathf.FloorToInt(GrowthPercentage / growthInterval), properties.Count - 1);
            Grew?.Invoke(this);
        }
        else
        {
            Stage = 0;
        }
    }

    public void Grow(float amount)
    {
        growthElapsed += amount;

        if (growthElapsed >= goalTime)
        {
            Completed?.Invoke(this);
        }

        UpdateStage();
    }

    void UpdateStage()
    {
        if (GrowthPercentage > growthInterval * (Stage + 1))
        {
            if (Stage == properties.Count - 1) return;

            Stage++;

            Grew?.Invoke(this);
        }
    }
}