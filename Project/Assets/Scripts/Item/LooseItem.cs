using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseItem : MonoBehaviour
{
    const float GRAVITY = 2.25f;
    [SerializeField] float maxInitialForce;
    [SerializeField] float pickupDelay;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Collider2D col;
    [SerializeField] Rigidbody2D rb;
    ItemContainer item;
    bool pickingUp;
    bool grounded;
    float ground;

    public void Setup(Item itemType, int count, bool doForce = true)
    {
        item = new ItemContainer(itemType, count, null, sr, null);

        StartCoroutine(PickupDelay());

        if (doForce)
        {
            ground = transform.position.y + Random.Range(-.2f, 0f);
            //Vector2 force = new Vector2(Random.Range(0, 2) == 0 ? Random.Range(initialForce.x, initialForce.y) : Random.Range(-initialForce.y, -initialForce.x), Random.Range(0, 2) == 0 ? Random.Range(initialForce.x, initialForce.y) : Random.Range(-initialForce.y, -initialForce.x));
            Vector2 force = new Vector2(Random.Range(-maxInitialForce / 3f, maxInitialForce / 3f), Random.Range(maxInitialForce / 2f, maxInitialForce));
            rb.AddForce(force);
            rb.angularVelocity = Random.Range(-90f, 90f);
            rb.gravityScale = GRAVITY;
        }
    }

    IEnumerator PickupDelay()
    {
        yield return new WaitForSeconds(pickupDelay);
        col.enabled = true;
    }


    float lastY;

    private void FixedUpdate()
    {
        if (pickingUp) return;

        if (transform.position.y < ground)
        {
            if (!grounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(-rb.velocity.y * .999f, 0, Mathf.Infinity));
                grounded = true;
            }

            if (rb.velocity.y <= 0)
                transform.position = new Vector3(transform.position.x, lastY);

        }
        else
        {
            grounded = false;
        }

        lastY = transform.position.y;
        //rb.velocity -= new Vector2(0, Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerMagnet"))
        {
            rb.gravityScale = 0;
            pickingUp = true;
        }
        else if (collision.CompareTag("Player"))
        {
            int leftOver = collision.GetComponentInParent<Character>().PickupLooseItem(item);

            if (leftOver == 0)
            {
                Destroy(gameObject);
            }
            else
            {
                item.Count = leftOver;
            }
        }
        else if (collision.CompareTag("Enemy"))
            Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerMagnet"))
        {
            ground = transform.position.y - .1f;
            rb.gravityScale = GRAVITY;
            pickingUp = false;
        }
    }
}
