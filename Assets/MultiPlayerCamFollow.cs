using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultiPlayerCamFollow : MonoBehaviour
{
    private List<Transform> targets = new List<Transform>();
    public Vector3 offset;
    private Vector3 velocity;
    public float smoothTime = .25f;

    public float minZoom = 5f;
    public float maxZoom = 10f;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        //generate list of all players
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject targetObject in targetObjects)
        {
            targets.Add(targetObject.transform);
        }
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(targets.Count == 0)
        {
            return;
        }
        Move();
        Zoom();
    }
    void Zoom()
    {
        float newZoom = Mathf.Clamp(GetGreatestDistance()/1.5f, minZoom, maxZoom);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }
    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }
    Vector3 GetCenterPoint()
    {
        if(targets.Count <= 0)
        {
            return Vector3.zero;
        }
        else if (targets.Count ==1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }
    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        float greatestDist = Mathf.Sqrt(Mathf.Pow(bounds.size.x,2) + Mathf.Pow(bounds.size.y, 2));
        return greatestDist;
    }
}
