using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using Pathfinding;
    
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
        Vector3Int hitCell = tilemap.WorldToCell(hitPosition);
        if (tilemap.GetTile(hitCell) != null)
        {
            tilemap.SetTile(hitCell, null);
            RescanArea(hitPosition, new Vector3(1.5f, 1.5f, 1));
        }
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
                if (tilemap.GetTile(new Vector3Int(x, y, 0)) != null)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
        }
        RescanArea(center, new Vector3(radius + 1, radius + 1, 1));
    }

    public void RescanArea(Vector3 center, Vector3 dimensions)
    {
        Bounds rescanBounds = new Bounds(center, dimensions); 
        var guo = new GraphUpdateObject(rescanBounds);
        // Set some settings
        guo.updatePhysics = true;
        AstarPath.active.UpdateGraphs(guo);
    }
}
