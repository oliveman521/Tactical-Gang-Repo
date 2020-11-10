using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using Pathfinding;
    
public class DestructableTilemap : MonoBehaviour
{
    private Tilemap tilemap;
    private Dictionary<Vector3Int, float> damageMemory = new Dictionary<Vector3Int, float>();
    public float tileHealth = 2;
     
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void ShootBlock(Vector3 hitPosition,float shotDamage)
    {
        Vector3Int hitCell = tilemap.WorldToCell(hitPosition); //figure out whick cell the damage is in
        if(DamageTile(hitCell, shotDamage)) //deal damage to that block
        {
            RescanArea(hitPosition, new Vector3(1.2f, 1.2f, 1)); //rescan the area if the damage destroyed a block;
        }
    }
    public void ExplosionDamage(Vector3 center, float radius, float centerDamage)
    {
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
                Vector3Int currentCell = new Vector3Int(x, y, 0);
                float newDamage = centerDamage * (1 - (Vector3.Distance(tilemap.CellToWorld(currentCell), center) / radius));
                Mathf.Clamp(newDamage, 0, centerDamage);
                DamageTile(currentCell, newDamage);
            }
        }
        RescanArea(center, new Vector3(radius + 1, radius + 1, 1));
    }

    public bool DamageTile(Vector3Int cell, float newDamage)
    {
        //Thsi function returns whther or not it broke the tile. Breaking the tile will likely lead to a rescan.


        if(damageMemory.TryGetValue(cell, out float oldDamage)) //if we were already tracking damage on this tile.
        {
            float totalDamage = oldDamage + newDamage;
            if(totalDamage >= tileHealth)
            {
                tilemap.SetTile(cell, null);
                damageMemory.Remove(cell);
                return true;
            }
            else
            {
                damageMemory[cell] = totalDamage;
                return false;
            }
        }
        else  //if this tile didn't have damage before
        {
            if (newDamage >= tileHealth) //skip the whole dictonary nonsense if it's too much damage
            {
                tilemap.SetTile(cell, null);
                return true;
            }
            else
            {
                damageMemory.Add(cell, newDamage); //add a new dictionary entry for the amount of damage done
                return false;
            }
        }

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
