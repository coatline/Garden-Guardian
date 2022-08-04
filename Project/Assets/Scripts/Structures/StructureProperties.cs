using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StructureProperties
{
    [FoldoutGroup("Visuals")]
    [GUIColor(1, 0.9f, 0.9f)]
    [SerializeField] Sprite[] damagedSprites;

    [FoldoutGroup("Visuals")]
    [GUIColor(.8f, 0.8f, 1f)]
    [SerializeField] StructureRenderer prefab;

    [FoldoutGroup("Properties")]
    [GUIColor(.8f, 1f, .9f)]
    [SerializeField] ItemPool dropPool;

    [FoldoutGroup("Properties")]
    [GUIColor(.8f, 0.8f, 1f)]
    [SerializeField] List<ToolType> tools;

    [FoldoutGroup("Properties")]
    [GUIColor(1, 0.7f, 0.7f)]
    [SerializeField] int hitPoints = 1;

    public int HitPoints { get { return hitPoints; } }
    public ItemPool DropPool { get { return dropPool; } }
    public StructureRenderer Prefab { get { return prefab; } }
    public Sprite[] DamagedSprites { get { return damagedSprites; } }
    public bool CanHitWith(ToolType tool) { return tools.Contains(tool); }
}

//[FoldoutGroup("Properties")]