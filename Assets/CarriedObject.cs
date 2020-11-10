using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriedObject : MonoBehaviour
{
    public GameObject carriedObject;
    private Transform carriedTransform;
    private Collider2D carriedCollider;
    public LayerMask solidLayers;
    private ContactFilter2D solidFilter;
    public float moveSpeed = 2f;
    public float minY = .1f;
    private float maxY;

    private void Start()
    {
        carriedTransform = carriedObject.GetComponent<Transform>();
        carriedCollider = carriedObject.GetComponent<Collider2D>();
        maxY = carriedTransform.localPosition.y;
        solidFilter.SetLayerMask(solidLayers);
    }

    private void Update()
    {
        float hitNum = Physics2D.OverlapCollider(carriedCollider, solidFilter, new Collider2D[1]);
        if(hitNum > 0 && carriedTransform.localPosition.y > minY)
        {
            carriedTransform.Translate(new Vector3(0, - moveSpeed * Time.deltaTime, 0), Space.Self);
        }
        else if(hitNum == 0 && carriedTransform.localPosition.y < maxY)
        {
            carriedTransform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0), Space.Self);
        }
    }
}
