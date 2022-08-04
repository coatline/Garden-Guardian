using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Play shooting sound on projectiles so that fast firing weapons do not cut off the sounds

    [SerializeField] protected ParticleSystem particles;
    [SerializeField] protected Rigidbody2D rb;

    [SerializeField] Sprite playerBulletSprite;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Collider2D col;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Animator animator;
    [SerializeField] Sound soundOnHit;

    [SerializeField] Color playerStartColor;
    [SerializeField] int durability;


    protected bool playerAttack;
    protected float knockback;
    protected int damage;
    bool dead;

    public void Setup(Vector3 force, int damage, float knockback, SoundData soundOnShot)
    {
        rb.velocity = force;
        this.damage = damage;
        this.knockback = knockback;

        if (soundOnShot)
            audioSource.PlayOneShot(soundOnShot.sound.RandomSound());

        col.enabled = true;
    }

    public void ChangeToPlayerBullet()
    {
        playerAttack = true;
        sr.sprite = playerBulletSprite;

        if (particles)
        {
            var m = particles.main;
            m.startColor = new ParticleSystem.MinMaxGradient(playerStartColor);
        }

        gameObject.layer = 6;
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < .35f)
        {
            // Play fade out animation
            DestroyProjectile(true);
        }
    }

    public virtual void DestroyProjectile(bool fadeOut)
    {
        if (dead) { return; }
        dead = true;

        if (animator)
        {
            if (fadeOut)
            {
                animator.Play("Fade_Out");
            }
            else
            {
                //ads.PlayOneShot(soundOnHit.sound.GetClip());
                animator.Play("Bullet_Explode");
            }
        }
        else
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void Emit()
    {
        particles.Emit(5);
    }

    public void Hit()
    {
        if (soundOnHit != null)
            audioSource.PlayOneShot(soundOnHit.RandomSound());

        durability--;

        if (durability <= 0)
        {
            // Play explode animation
            DestroyProjectile(false);
        }
    }
}