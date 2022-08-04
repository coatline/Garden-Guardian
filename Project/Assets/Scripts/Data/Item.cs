using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NEW ITEM JACK", menuName = "Item")]
public class Item : ScriptableObject
{
    [SerializeField] ItemType type;
    [SerializeField] ToolType toolType;

    [ToggleGroup("IsWeapon")]
    [SerializeField] bool IsWeapon;

    [ToggleGroup("IsWeapon")]
    [SerializeField] Weapon weapon;
    [SerializeField] Sprite sprite;
    [SerializeField] SoundData soundOnUse;
    [SerializeField] Structure structure;
    [TextArea(0, 3)]
    [SerializeField] string description;

    [SerializeField] bool autoUse;
    [SerializeField] float useRate;
    [SerializeField] int maxStack;
    [SerializeField] int healAmount;

    [FoldoutGroup("Recoil")]
    [SerializeField] float recoilAmount;
    [FoldoutGroup("Recoil")]
    [SerializeField] float recoilRecoveryDelay;
    [FoldoutGroup("Recoil")]
    [SerializeField] float recoilRecoverySpeed;

    public Structure Structure { get { return structure; } }
    public int HealAmount { get { return healAmount; } }
    public Weapon Weapon { get { return weapon; } }
    public SoundData SoundOnUse { get { return soundOnUse; } }
    public int MaxStack { get { return maxStack; } }
    public float UseRate { get { return useRate; } }
    public bool AutoUse { get { return autoUse; } }
    public Sprite Sprite { get { return sprite; } }
    public ItemType Type { get { return type; } }
    public ToolType ToolType { get { return toolType; } }
    public string Description { get { return description; } }
    public float RecoilAmount { get { return recoilAmount; } }
    public float RecoilRecoveryDelay { get { return recoilRecoveryDelay; } }
    public float RecoilRecoverySpeed { get { return recoilRecoverySpeed; } }

    public string Name => name;
}

public enum ToolType
{
    None,
    Hoe
}

public enum ItemType
{
    Gun,
    Melee,
    Seeds,
    Tool,
    Health,
    Structure,
    Generic
}