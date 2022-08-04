using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExplosionManager : MonoBehaviour
{
    [SerializeField] Explosion explosionPrefab;
    [SerializeField] Tilemap wallTilemap;

    public void ExplodeAt(Vector2 pos, int damage, int breakWallsInRadius)
    {
        var e = Instantiate(explosionPrefab, pos, Quaternion.identity);
        e.GetComponent<Explosion>().damage = damage;
        e.GetComponentInChildren<Explosion>().damage = damage;
        DestroyWalls(pos, breakWallsInRadius);
    }

    void DestroyWalls(Vector2 pos, int radius)
    {
        //var tileSize = 1;
        //float blx = ((pos.x) / tileSize);
        //float bly = ((pos.y) / tileSize);

        //for (float x = blx - radius; x <= blx + radius; x++)
        //    for (float y = bly - radius; y <= bly + radius; y++)
        //    {
        //        var p = new Vector3Int((int)x, (int)y, 0);

        //        if (wallTilemap.GetTile(p) != null)
        //        {
        //            wallTilemap.SetTile(p, DataManager.I.CurrentLevelData().brokenWallTile);
        //        }
        //    }
    }
}
