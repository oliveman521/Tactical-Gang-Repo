﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using CodeMonkey.Utils;

[RequireComponent(typeof(PlayerBase))]
public class DemoAbilities : MonoBehaviour
{
    private PlayerBase playerBase;
    public GameObject detPackPrefab;
    public Transform packSpawnPoint;
    public List<GameObject> detPacks = new List<GameObject>();
    public float placingRange = 2;
    public float explosionRadius;
    public float explosionDamage;
    public LayerMask wallLayers;
    private ContactFilter2D wallFilter;
    // Start is called before the first frame update
    void Start()
    {
        playerBase = GetComponent<PlayerBase>();
        wallFilter.layerMask = wallLayers;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LayDetPack(CallbackContext ctx)
    { 
        if(ctx.started && playerBase.down == false)
        {
            GameObject newDetPack;
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, placingRange, wallLayers);
            if(hitInfo)
            {
                float rotationToBeFlatToWall = UtilsClass.GetAngleFromVector(hitInfo.normal) + 90;
                newDetPack = Instantiate(detPackPrefab, hitInfo.point, Quaternion.Euler(new Vector3(0, 0, rotationToBeFlatToWall)));
                newDetPack.transform.parent = hitInfo.collider.gameObject.transform;
            }
            else
            {
                newDetPack = Instantiate(detPackPrefab, transform.position + transform.up*placingRange/2, transform.rotation);
            }

            detPacks.Add(newDetPack);
        }
    }
    public void DetonateDetPacks(CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (detPacks.Count > 0)
            {
                foreach (GameObject detPack in detPacks)
                {
                    if(detPack != null) //Detpack might be null if it got destroyed early from say sticking it to an enemy, and that enemy getting killed
                    {
                        Collider2D[] results = new Collider2D[10];
                        Vector3 explosionCenter = detPack.transform.position + transform.up * explosionRadius / 3;
                        Physics2D.OverlapCircle(explosionCenter, explosionRadius, wallFilter, results);
                        for (int i = 0; i < results.Length; i++)
                        {
                            try
                            {
                                results[i].GetComponent<DestructableTilemap>().ExplosionDamage(explosionCenter, explosionRadius, explosionDamage);
                            }
                            catch { }
                            try
                            {
                                results[i].GetComponent<EnemyBehavior2>().Damage(explosionDamage, null);
                            }
                            catch { }
                        }
                        Destroy(detPack);
                    }
                }
                detPacks = new List<GameObject>();
            }
        }
    }
}

