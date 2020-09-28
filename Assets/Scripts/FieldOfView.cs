using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Diagnostics;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask solidsMask;
    private Mesh mesh;
    private Vector3 origin = Vector3.zero;
    private float startingAngle = 0;
    public float fov = 90f;
    public float viewDistance = 20f;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //maintain home location
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        float wallViewDist = .125f;
        Vector3 origin = this.origin;
        int raycount = (int)fov;
        float angle = startingAngle;
        float angleIncrease = fov / raycount;

        Vector3[] verticies = new Vector3[raycount + 1 + 1];
        Vector2[] uv = new Vector2[verticies.Length];
        int[] triangles = new int[raycount * 3];

        verticies[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= raycount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, UtilsClass.GetVectorFromAngle(angle), viewDistance, solidsMask);
            if (raycastHit2D.collider == null)
            {
                //no hit
                vertex = origin + UtilsClass.GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                //hit something
                vertex = raycastHit2D.point + (Vector2)(UtilsClass.GetVectorFromAngle(angle).normalized * wallViewDist);
            }
            verticies[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = verticies;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(float aimDirection)
    {
        startingAngle = (aimDirection) + fov / 2;
    }
}
