using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualPriorityPass : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] SpriteSorter ss;
    [SerializeField] float alpha;

    List<SpriteRenderer> seenRenderers;

    private void Start()
    {
        seenRenderers = new List<SpriteRenderer>();
        ss.OrderChanged += OrderChanged;
    }

    void OrderChanged()
    {
        for (int i = 0; i < seenRenderers.Count; i++)
        {
            SpriteRenderer sr = seenRenderers[i];

            if (sr.sortingOrder >= this.sr.sortingOrder)
            {
                sr.color = Color.white - new Color(0, 0, 0, 1 - alpha);
            }
            else
            {
                sr.color = Color.white;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject.transform.parent.gameObject;

        SpriteRenderer sr = null;

        if (go)
            sr = go.GetComponent<SpriteRenderer>();

        if (sr && seenRenderers.Contains(sr) == false)
        {
            seenRenderers.Add(sr);

            if (sr.sortingOrder >= this.sr.sortingOrder)
            {
                sr.color = Color.white - new Color(0, 0, 0, 1 - alpha);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject go = collision.gameObject.transform.parent.gameObject;
        SpriteRenderer sr = null;

        if (go)
            sr = go.GetComponent<SpriteRenderer>();

        if (sr)
        {
            sr.color = Color.white;
            seenRenderers.Remove(sr);
        }
    }

    class SeenRenderer
    {
        SpriteRenderer sr;
        bool active;

        public SeenRenderer(SpriteRenderer sr, bool active)
        {
            this.active = active;
            this.sr = sr;
        }
    }
}
