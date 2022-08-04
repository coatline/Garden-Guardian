using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseItemSpawner : MonoBehaviour
{
    [SerializeField] LooseItem looseItemPrefab;

    #region Statics
    static LooseItemSpawner instance;
    public static LooseItemSpawner I
    {
        get
        {
            return instance;
        }
        set
        {
            if (instance) { return; }
            else
            {
                instance = value;
            }
        }
    }
    #endregion

    private void Awake()
    {
        #region Statics
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning($"Already a {name} in scene. Deleting this one!");
            Destroy(gameObject);
            return;
        }
        #endregion
    }

    public void SpawnItem(Item i, int count, Vector3 pos, bool doForce = true)
    {
        LooseItem li = Instantiate(looseItemPrefab, pos, Quaternion.identity, transform);
        li.Setup(i, count, doForce);
    }

    public void SpawnItems(ItemPool itemPool, Vector3 pos, bool doForce = true)
    {
        ItemPackage[] items = itemPool.GetItems();

        for (int k = 0; k < items.Length; k++)
        {
            LooseItem li = Instantiate(looseItemPrefab, pos, Quaternion.identity, transform);
            li.Setup(items[k].Item, items[k].Count, doForce);
        }
    }
}
