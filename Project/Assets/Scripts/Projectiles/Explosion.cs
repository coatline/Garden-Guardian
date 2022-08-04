using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int damage;

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var d = collision.GetComponent<IDamageable>();

        if (d != null)
        {
            d.Damage(damage, (collision.transform.position - transform.position).normalized);
        }
    }
}
