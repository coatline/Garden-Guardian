using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Plant")]

public class Plant : Structure
{
    [BoxGroup("Basic")]
    [SerializeField] float growTime;

    [BoxGroup("Basic")]
    [SerializeField] bool requiresTilledSoil = true;

    [BoxGroup("Basic")]
    [SerializeField] List<StructureProperties> growthProperties;


    [BoxGroup("Maturity")]
    [SerializeField] bool reproduces;

    [BoxGroup("Maturity")]
    [SerializeField] float produceTime;

    //[BoxGroup("Maturity")]
    //[SerializeField] bool showFinalVisualWhenMature = true;

    [BoxGroup("Maturity")]
    [SerializeField] List<StructureProperties> matureProperties;

    public float GrowTime { get { return growTime; } }
    public bool Reproduces { get { return reproduces; } }
    public float ProduceTime { get { return produceTime; } }
    public bool RequiresTilledSoil { get { return requiresTilledSoil; } }
    //public bool FinalStageWhenMature { get { return showFinalVisualWhenMature; } }
    public List<StructureProperties> GrowthProperties { get { return growthProperties; } }
    public List<StructureProperties> MatureProperties { get { return matureProperties; } }
}
