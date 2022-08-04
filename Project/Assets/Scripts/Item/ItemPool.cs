using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ItemPool
{
    [SerializeField] ItemSpawnProperties[] pool;

    [SerializeField] Vector2Int dropCountRange = Vector2Int.one;
    int total = -1;

    public ItemPackage[] GetItems()
    {
        int dropCount = Random.Range(dropCountRange.x, dropCountRange.y + 1);

        ItemPackage[] items = new ItemPackage[dropCount];

        int index = 0;

        for (int k = 0; k < pool.Length; k++)
        {
            for (int j = 0; j < pool[k].requiredDrops; j++)
            {
                items[index] = new ItemPackage(pool[k].item, Random.Range(pool[k].countPerDrop.x, pool[k].countPerDrop.y));
                index++;
            }
        }

        for (int i = index; i < dropCount; i++)
        {
            items[i] = GetItem();
        }

        return items;
    }

    public ItemPackage GetItem(/*ItemSpawnProperties[] pool*/)
    {
        // With this method of storing the total for performance, I cannot have dynamic pools

        if (total == -1)
            total = pool.Sum(a => a.spawnWeight);

        float threshold = Random.Range(0f, total);

        for (int i = 0; i < pool.Length; i++)
        {
            if (pool[i].spawnWeight > threshold)
            {
                return new ItemPackage(pool[i].item, Random.Range(pool[i].countPerDrop.x, pool[i].countPerDrop.y));
            }
            else
            {
                threshold -= pool[i].spawnWeight;
            }
        }

        return null;
    }
}

[System.Serializable]
public class ItemSpawnProperties
{
    public Vector2Int countPerDrop = Vector2Int.one;
    public int requiredDrops;
    public int spawnWeight;
    public Item item;
    //public int count;
}