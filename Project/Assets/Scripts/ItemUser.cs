using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUser : MonoBehaviour
{
    public event System.Action<int> Healed;

    [SerializeField] MuzzleFlashAnimation muzzleFlashAnimation;
    [SerializeField] AudioSource itemAudioSource;
    [SerializeField] SpriteRenderer muzzleFlash;
    [SerializeField] SpriteRenderer itemSprite;
    [SerializeField] Transform handTransform;
    [SerializeField] RecoilAnimation recoil;
    [SerializeField] ItemHolder itemHolder;
    [SerializeField] bool player;
    bool canUseItem = true;

    ItemContainer itemContainer;
    World world;

    private void Awake()
    {
        itemHolder.SwitchedItem += SwitchedItem;
        world = WorldController.I.World;
    }

    void SwitchedItem(ItemContainer itemC)
    {
        this.itemContainer = itemC;
    }

    IEnumerator ItemUseTimer()
    {
        if (!itemContainer.ItemType) { yield return null; }
        canUseItem = false;
        //float useTime = itemContainer.ItemType.UseRate;
        yield return new WaitForSeconds(useTime);
        canUseItem = true;
    }

    // Separate variable just because we could deplete our item
    // when we use it and cause a null reference exception
    float useTime;

    public void TryUseItem(WorldCell cell)
    {
        if (itemContainer == null) return;
        if (!canUseItem) return;

        useTime = itemContainer.ItemType.UseRate;

        switch (itemContainer.ItemType.Type)
        {
            case ItemType.Gun:

                if (itemContainer.ItemType.Weapon.Burst)
                {
                    StartCoroutine(Burst());
                }
                else
                {
                    for (int i = 0; i < itemContainer.ItemType.Weapon.AttackCount; i++)
                    {
                        Shoot(i);
                        recoil.Recoil();
                    }
                }

                // Recoil is handled in the shooting so we don't
                // want to continue down.
                StartCoroutine(ItemUseTimer());
                return;

            case ItemType.Seeds:

                Plant p = itemContainer.ItemType.Structure as Plant;

                if (cell.Plant != null) return;

                // If there is no structure or the structure is a pot or something. 
                if (cell.Structure == null || (cell.Structure != null && cell.Structure.Data.CanPlantOn))
                {
                    // If the ground is suitable for planting.
                    if (cell.Ground.Tilled == true || !p.RequiresTilledSoil)
                    {
                        cell.Plant = new WorldPlant(itemContainer.ItemType.Structure as Plant, 0, cell);
                        itemContainer.Count--;
                    }
                    else
                        return;
                }
                else
                    return;

                break;

            case ItemType.Tool:

                // If it doesn't do anything, do not reset use timer or recoil hand.
                if (!UseTool(cell, itemContainer.ItemType))
                    return;

                break;

            case ItemType.Melee:

                if (itemContainer.ItemType.Weapon.Burst)
                {
                    StartCoroutine(Burst());
                }
                else
                {
                    for (int i = 0; i < itemContainer.ItemType.Weapon.AttackCount; i++)
                    {
                        Shoot(i);
                    }
                }

                break;

            case ItemType.Health:

                Healed?.Invoke(itemContainer.ItemType.HealAmount);

                //itemAudioSource.PlayOneShot(itemContainer.ItemType.SoundOnUse.sound.RandomSound());

                itemContainer.Count--;

                break;

            case ItemType.Structure:

                if (cell.Plant != null || cell.Structure != null) return;

                cell.Structure = new WorldObject(itemContainer.ItemType.Structure, cell);
                itemContainer.Count--;

                break;
        }

        StartCoroutine(ItemUseTimer());
        recoil.Recoil();
    }

    void Shoot(int bulletIndex)
    {
        Item item = itemContainer.ItemType;

        float randRot = 0;
        float xOffset = 0;

        if (!itemContainer.ItemType.Weapon.ParellelBullets)
        {
            float spread = (((float)item.Weapon.AttackCount * (float)item.Weapon.AttackSpacing) / 2f);
            float weaponSpreadVal = item.Weapon.Spread;

            randRot = -(spread) + Random.Range(-weaponSpreadVal, weaponSpreadVal) + ((float)bulletIndex * item.Weapon.AttackSpacing);
        }
        else
        {
            xOffset = -((item.Weapon.AttackCount * item.Weapon.AttackSpacing) / 2) + (bulletIndex * item.Weapon.AttackSpacing);
        }

        CreateProjectile(randRot, xOffset);
    }

    IEnumerator Burst()
    {
        Item item = itemContainer.ItemType;

        float burstTime = item.Weapon.TimeBetweenAttacks;
        int bullets = item.Weapon.AttacksPerBurst;

        for (int i = 0; i < bullets; i++)
        {
            // If we change items partway through the burst
            if (item != itemContainer.ItemType) { break; }

            recoil.Recoil();

            for (int k = 0; k < item.Weapon.AttackCount; k++)
            {
                Shoot(k);
            }

            yield return new WaitForSeconds(burstTime);
        }
    }

    Projectile CreateProjectile(float randRot, float xOffset)
    {
        Item i = itemContainer.ItemType;

        if (i.Weapon.Gun.MuzzleFlash)
        {
            muzzleFlashAnimation.Flash(i.Weapon.Gun.MuzzleFlashSpeed, i.Weapon.Gun.MuzzleFlashSize, i.Weapon.Gun.MuzzleFlashAlpha);
        }

        var bulletHole = recoil.GetOffsetFromHand(new Vector2(i.Weapon.AttackOffset.x, i.Weapon.AttackOffset.y));

        muzzleFlash.transform.position = bulletHole + itemSprite.transform.position;

        Projectile newProjectile = Instantiate(i.Weapon.ProjectilePrefab, itemSprite.transform.position, Quaternion.Euler(handTransform.eulerAngles - new Vector3(0, 0, 90 + randRot)));

        newProjectile.transform.localPosition += new Vector3(bulletHole.x, bulletHole.y);
        newProjectile.transform.Translate(newProjectile.transform.right * xOffset, Space.World);

        if (player)
            newProjectile.ChangeToPlayerBullet();

        newProjectile.Setup(newProjectile.transform.up * i.Weapon.AttackForce, i.Weapon.Damage, i.Weapon.Knockback, i.SoundOnUse);

        return newProjectile;
    }

    bool TryHitStructureAt(int x, int y, int damage, ToolType tool = ToolType.None)
    {
        WorldCell c = world.GetCell(x, y);

        //if (c.Plant != null)
        //{
        //    if (c.Plant.CurrentProperties.CanHitWith(tool))
        //    {
        //        c.Plant.TakeDamage(damage);
        //        return true;
        //    }
        //}
        //else
        if (c.Structure != null)
        {
            if (c.Structure.Data.Properties.CanHitWith(tool))
            {
                c.Structure.TakeDamage(damage);
                return true;
            }
        }

        return false;
    }

    public bool UseTool(WorldCell cell, Item item)
    {
        switch (item.ToolType)
        {
            //case ToolType.Pickaxe: case ToolType.Axe: TryHitStructureAt(x, y, item.ToolPower, item.ToolType); break;

            case ToolType.Hoe:

                //if (cell.Structure != null || cell.Plant != null)
                //{
                //    if (TryHitStructureAt(x, y, item.ToolPower, item.ToolType))
                //    {
                //        break;
                //    }
                //}
                //else
                {
                    return TillTileAt(cell.X, cell.Y);
                }

                break;
        }

        return false;
    }

    bool TillTileAt(int x, int y)
    {
        WorldCell c = world.GetCell(x, y);
        Ground tilled = DataLibrary.I.Grounds["Tilled Soil"] as Ground;

        if (c.Ground == tilled)
            return false;

        c.Ground = tilled;
        return true;
    }
}
