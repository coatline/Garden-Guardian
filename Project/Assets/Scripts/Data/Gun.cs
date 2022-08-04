using UnityEngine;

[System.Serializable]
public class Gun
{
    [SerializeField] float muzzleFlashAlpha;
    [SerializeField] float muzzleFlashSpeed;
    [SerializeField] float muzzleFlashSize;
    [SerializeField] bool muzzleFlash;

    public float MuzzleFlashSpeed { get { return muzzleFlashSpeed; } }
    public float MuzzleFlashSize { get { return muzzleFlashSize; } }
    public float MuzzleFlashAlpha { get { return muzzleFlashAlpha; } }
    public bool MuzzleFlash { get { return muzzleFlash; } }

    //public float bulletSpeed;
    //public int bulletCount = 1;
    //public float spread;
    //public float bulletHoleX;
    //public float bulletHoleY;

    //[Header("Burst")]
    //public bool burst;
    //public int shotsPerBurst;
    //public float timeBetweenBullets;

    //[Header("Multi Shot")]
    //public bool parallelBullets;
    //public float bulletSpacing = .3f;
}