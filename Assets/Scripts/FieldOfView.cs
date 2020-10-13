using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask solidsMask = ~0;
    private Mesh mesh;
    [Range(0,360)]
    public float fov = 90f;
    public float rayCount = 90;
    public float viewDistance = 20f;
    public int edgeResolveIterations = 4;
    public float edgeDistThreshold = 1;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        mesh.name = "FOV mesh";
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //maintain home location
        //transform.position = Vector3.zero;
        //transform.rotation = Quaternion.identity;

        float wallViewDist = .25f;
        float angle = fov / 2 + 90;
        float angleStepSize = fov / (rayCount - 1);
        ViewCastInfo oldViewCast = new ViewCastInfo();
        List<Vector3> fovPoints = new List<Vector3>();

        for (int i = 0; i < rayCount; i++)
        {
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0) //advanced edge detection
            {
                bool edgeDistThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDistThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        fovPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        fovPoints.Add(edge.pointB);
                    }
                }
            }
            fovPoints.Add(newViewCast.point); //add a new point to our list for the new raycast
            angle -= angleStepSize; //progress the angle
            oldViewCast = newViewCast;
            
        }

        int vertexCount = fovPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;


        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(fovPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDistThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDistThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDistThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else 
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }
        return new EdgeInfo(minPoint, maxPoint);
    }
    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
        
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(angleIsGlobal != false)
        {
            angleInDegrees += transform.eulerAngles.z;  //if angle isnt in global space, convert it to be in global space.
        }
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }
    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, dir, viewDistance, solidsMask);
        
        

        if (hitInfo)
        {
            //change layer of crate so that it was totally visable
            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Obsticles"))
            {
                hitInfo.collider.gameObject.layer = LayerMask.NameToLayer("Walls");
            }

            return new ViewCastInfo(true, hitInfo.point, hitInfo.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewDistance, solidsMask, globalAngle);
        }
        
    }
}
