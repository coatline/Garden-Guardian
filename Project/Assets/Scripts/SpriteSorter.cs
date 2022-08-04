using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] float yOffset;
    [SerializeField] bool once;

    public event System.Action OrderChanged;

    public float YOffset { get { return yOffset; } }

    private void Start()
    {
        if (once)
        {
            sr.sortingOrder = -(int)(transform.position.y);
            Destroy(this);
        }
    }

    int prevX;

    void Update()
    {
        int order = -(int)(transform.position.y + yOffset);
        int x = (int)transform.position.x;

        if (sr.sortingOrder != order)
        {
            sr.sortingOrder = order;
            OrderChanged?.Invoke();
        }
        else if (prevX != x)
        {
            OrderChanged?.Invoke();
            prevX = x;
        }
    }
}
