using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashWhiteAnimation : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] float duration;
    Material material;

    void Start()
    {
        material = sr.material;
    }

    float timer;

    void Update()
    {
        if (timer > duration)
            return;

        timer += Time.deltaTime;

        material.SetFloat("_FlashAmount", curve.Evaluate((timer / duration)));
    }

    public void Flash()
    {
        timer = 0;
    }
}
