using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public event System.Action<ItemContainer> SwitchedItem;

    [SerializeField] SpriteRenderer muzzleFlash;
    [SerializeField] SpriteRenderer itemSprite;

    [SerializeField] Transform handTransform;
    [SerializeField] Transform handSprite;

    [SerializeField] RecoilAnimation recoil;

    [SerializeField] ItemUser itemUser;

    public ItemContainer ItemContainer { get; private set; }

    [SerializeField] float reach;

    public void ChangeItem(ItemContainer i)
    {
        ItemContainer?.SetSprite(null);
        ItemContainer = i;
        ItemContainer.SetSprite(itemSprite);

        SwitchedItem?.Invoke(ItemContainer);
    }

    /// <summary>
    /// Aims the hand and item towards designated position.
    /// </summary>
    /// <param name="toPosition">The position you want to aim at.</param>
    /// <param name="aimVariability">For variability in AI attacks.</param>
    public void Aim(Vector3 toPosition, Vector2 aimVariability)
    {
        float angle = Extensions.AngleFromPosition(transform.position, toPosition);

        angle += Random.Range((float)aimVariability.x, (float)aimVariability.y);

        float flip = 0;

        if (angle > 0 || angle < -180)
        {
            flip = 180;
        }

        Vector2 pos = (toPosition - transform.position).normalized * (Mathf.Clamp(Vector3.Distance(transform.position, toPosition), 0f, reach));

        handTransform.localPosition = pos;

        handTransform.transform.localRotation = Quaternion.Euler(0, 0, (angle + 90));
        handSprite.transform.localRotation = Quaternion.Euler(flip, 0, 0);
    }
}