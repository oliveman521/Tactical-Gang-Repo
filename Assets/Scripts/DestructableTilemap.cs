using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}
