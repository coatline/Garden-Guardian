using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashAnimation : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;

    float speed;
    float alpha;

    public void Flash(float speed, float size, float alpha)
    {
        this.speed = speed;

        sr.transform.localScale = Vector3.one * size;
        this.alpha = alpha;
    }

    void Update()
    {
        if (alpha > 0)
        {
            alpha -= Time.deltaTime * speed;
            sr.color = new Color(1, 1, 1, alpha);
        }
    }
}
