using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class DestructableTilemap : MonoBehaviour
{
    private Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void BreakBlock(Vector3 hitPosition)
    {
        //Debug.Log("Break at point" + hitPosition);
        tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
    }
    public void ExlosionBlockDamage(Vector3 center, float radius)
    {
        Debug.Log("Explosion at" + center);
        Vector3Int centerCell = tilemap.WorldToCell(center);
        int cellRadius = (int)(radius * 4);
        int minX = (int)centerCell.x - cellRadius;
        int maxX = (int)centerCell.x + cellRadius;
        int minY = (int)centerCell.y - cellRadius;
        int maxY = (int)centerCell.y + cellRadius;
        for (int x = minX; x < maxX+1; x++)
        {
            for (int y = minY; y < maxY + 1; y++)
            {
                tilemap.SetTile(new Vector3Int(x,y,0), null);
            }
        }

    }
}
