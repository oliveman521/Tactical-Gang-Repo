using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class WalkwayFavorability : MonoBehaviour
{
    private Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        for (int x = 0; x < tilemap.size.x; x++)
        {
            for (int y = 0; y < tilemap.size.y; y++)
            {
                Vector3Int cell = new Vector3Int(x, y, 0);
                if (tilemap.GetTile(cell) != null)
                {
                    Vector3 worldPoint = tilemap.CellToWorld(cell);
                    GraphNode node = AstarPath.active.GetNearest(worldPoint).node;
                    node.Tag = 1;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
