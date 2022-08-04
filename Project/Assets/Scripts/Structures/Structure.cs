using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Structure", menuName = "Structure")]

public class Structure : ScriptableObject
{
    [ShowInInspector]
    [ToggleGroup("Inventory")]
    [SerializeField] bool Inventory;

    [ShowInInspector]
    [ToggleGroup("Inventory")]
    [SerializeField] int slotCount;

    [SerializeField] bool canPlantOn;

    //[FoldoutGroup("Properties")]
    [GUIColor(1, 0.7f, 1f)]
    [BoxGroup("Properties")]
    [SerializeField] StructureProperties properties;

    [PropertySpace(SpaceBefore = 0, SpaceAfter = 60), PropertyOrder(2)]

    public StructureProperties Properties { get { return properties; } }
    public bool HasInventory { get { return Inventory; } }
    public int SlotCount { get { return slotCount; } }
    public bool CanPlantOn { get { return canPlantOn; } }

    public string Name => name;
}
