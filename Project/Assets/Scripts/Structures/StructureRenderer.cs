using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureRenderer : MonoBehaviour
{
    [SerializeField] GameObject rootGob;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sr;

    public GameObject RootGob { get { return rootGob; } }

    public void SetSprite(Sprite s)
    {
        sr.sprite = s;
    }

    public void PlayAnimation(string s)
    {
        if (animator == null) return;

        animator.enabled = true;
        animator.Play(s);
    }

    public void PlayHit()
    {
        if (animator == null) return;

        animator.enabled = true;
        animator.Play("Hit");
    }

    public void Finished()
    {
        animator.enabled = false;
    }
}
