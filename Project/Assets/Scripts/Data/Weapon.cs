using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class Weapon
{
    [SerializeField] Vector2 attackOffset;
    [SerializeField] float attackForce;
    [SerializeField] float attackCount;
    [SerializeField] float knockBack;
    [SerializeField] float spread;
    [SerializeField] int damage;

    [FoldoutGroup("Gun")]
    [SerializeField] Gun gun;
    [FoldoutGroup("Projectile")]
    [SerializeField] Projectile projectilePrefab;
    [FoldoutGroup("Projectile")]
    [SerializeField] Vector2 projectileScale;

    

    [FoldoutGroup("Burst")]
    [SerializeField] bool burst;
    [FoldoutGroup("Burst")]
    [SerializeField] int attacksPerBurst;
    [FoldoutGroup("Burst")]
    [SerializeField] float timeBetweenAttacks;

    [FoldoutGroup("Multi-Shot")]
    [SerializeField] bool parellelBullets;
    [FoldoutGroup("Multi-Shot")]
    [SerializeField] float attackSpacing = .3f;

    public Gun Gun { get { return gun; } }
    public Projectile ProjectilePrefab { get { return projectilePrefab; } }
    public Vector2 ProjectileScale { get { return projectileScale; } }
    public int Damage { get { return damage; } }
    public float AttackForce { get { return attackForce; } }
    public float AttackCount { get { return attackCount; } }
    public float Knockback { get { return knockBack; } }
    
    public float Spread { get { return spread; } }
    public Vector2 AttackOffset { get { return attackOffset; } }
    public bool Burst { get { return burst; } }
    public int AttacksPerBurst { get { return attacksPerBurst; } }
    public float TimeBetweenAttacks { get { return timeBetweenAttacks; } }
    public bool ParellelBullets { get { return parellelBullets; } }
    public float AttackSpacing { get { return attackSpacing; } }
}