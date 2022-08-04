using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class CursorInventory : MonoBehaviour
{
    [SerializeField] GameObject descriptionScreen;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text itemNameText;

    [SerializeField] TMP_Text countText;
    [SerializeField] Image itemImage;
    public ItemContainer holder;
    Camera cam;

    void Awake()
    {
        holder = new ItemContainer(null, 0, itemImage, null, countText);
        cam = Camera.main;
    }

    void Update()
    {
        if (holder.ItemType && Input.GetMouseButtonDown(1) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            LooseItemSpawner.I.SpawnItem(holder.ItemType, holder.Count, cam.ScreenToWorldPoint(transform.position) + new Vector3(0, 0, 10));
            holder.Count = 0;
        }

        transform.position = Input.mousePosition;
    }

    //ItemHolder displaying;

    public void MouseOver(ItemContainer i)
    {
        if (i == null || i.ItemType == null) return;

        descriptionScreen.SetActive(true);
        itemNameText.text = i.ItemType.Name;
        descriptionText.text = i.ItemType.Description;
    }

    public void UnMouseOver(ItemContainer i)
    {
        descriptionScreen.SetActive(false);
    }
}