using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeofVisionDrawer : MonoBehaviour
{
    private PolygonCollider2D polyCollider;
    public float length;
    public float width;
    public float characterRadius = .3125f;
    // Start is called before the first frame update
    void Start()
    {
        polyCollider = GetComponent<PolygonCollider2D>();
        var myPoints = polyCollider.points;
        myPoints[0] = new Vector2(characterRadius, characterRadius);
        myPoints[1] = new Vector2(-characterRadius, characterRadius);
        myPoints[2] = new Vector2(-width / 2, length + characterRadius);
        myPoints[3] = new Vector2(width / 2, length + characterRadius);
        polyCollider.points = myPoints;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
