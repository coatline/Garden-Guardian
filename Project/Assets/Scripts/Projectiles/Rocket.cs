using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Rocket : Bullet
{
    [SerializeField] GameObject explosion;

    void Start()
    {
        StartCoroutine(DelayMovement());
    }

    public override void DestroyProjectile(bool fadeOut)
    {
        FindObjectOfType<ExplosionManager>().ExplodeAt(transform.position, damage, 2);
        Destroy(gameObject);
    }

    Vector2 vel;

    private void Update()
    {
        if (rb.velocity.magnitude < vel.magnitude)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, vel, Time.deltaTime * 4);
        }
    }

    IEnumerator DelayMovement()
    {
        yield return new WaitForEndOfFrame();
        var vel = rb.velocity;
        rb.velocity -= new Vector2(rb.velocity.x / 3, rb.velocity.y / 3);
        yield return new WaitForSeconds(.25f);
        this.vel = vel;
    }
}
