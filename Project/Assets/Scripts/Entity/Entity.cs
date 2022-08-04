using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    [SerializeField] protected SpriteRenderer sr;
    [SerializeField] ParticleSystem healps;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator a;

    [SerializeField] protected int maxHealth;
    [SerializeField] float speed;
    protected bool dead;
    bool takingKnockback;
    protected int health;

    public event System.Action<Entity> Died;

    protected virtual void Awake()
    {
        health = maxHealth;
    }

    public virtual void Die()
    {
        Died?.Invoke(this);
        Destroy(gameObject);
    }

    bool moving;

    //protected void Move(Vector3 inputs)
    //{

    //}

    protected void MoveTowardsPos(Vector3 pos, bool isInputs = false)
    {
        if (dead) return;
        if (takingKnockback) return;

        if (!moving)
        {
            a.SetBool("Moving", true);
            moving = true;
        }

        Vector3 movement;

        if (isInputs)
            movement = pos.normalized;
        else
            movement = (pos - transform.position).normalized;

        rb.velocity = movement * speed * Time.fixedDeltaTime;

        if (movement.x > 0)
            sr.flipX = false;
        else if (movement.x < 0)
            sr.flipX = true;
    }

    void TakeKnockback(Vector2 force)
    {
        StopMoving();
        takingKnockback = true;
        rb.velocity = Vector2.zero;
        rb.velocity += force;
        StartCoroutine(Knockback());
    }

    IEnumerator Knockback()
    {
        yield return new WaitForSeconds(.1f);
        takingKnockback = false;
    }

    public void StopMoving()
    {
        if (moving)
        {
            moving = false;
            a.SetBool("Moving", false);
            rb.velocity = Vector2.zero;
        }
    }

    public virtual void Damage(int val, Vector2 knockBack)
    {
        health -= val;

        TakeKnockback(knockBack);
        rb.AddForce(knockBack);

        if (health <= 0)
        {
            a.Play("Die");
            StopMoving();
            dead = true;
        }
        else
            a.Play("Damage");
    }

    public virtual void Heal(int count)
    {
        health += count;
        if (health > maxHealth) { health = maxHealth; }
        healps.Play();
    }
}
