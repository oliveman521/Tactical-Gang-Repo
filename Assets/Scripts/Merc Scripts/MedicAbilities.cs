using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerBase))]
[RequireComponent(typeof(PlayerSupplyManager))]
public class MedicAbilities : MonoBehaviour
{
    [Header("References")]
    public Transform pulseField;
    public Transform pulseRing;
    public MeshFilter meshFilter;
    private Mesh mesh;
    private PlayerBase playerBase;
    private PlayerSupplyManager supplyManager;

    [Header("Pulse Settings")]
    [Tooltip("The layers that the healing pulse can affect. Should just be set to include players")]
    public LayerMask healableLayers;
    private float timeSinceChargeStart;
    public float maxChargeTime = 3;
    public float maxHealAmount = 5;
    public float healPerStep = .25f;
    private float healFromCurrentCharge = 0;
    public float suppliesPerHeal = 1;
    private float suppliesUsedByCharge = 0;

    public float pulseRadius = 4;
    public float timeToGrowPulse = .1f;
    private float timeSincePulse;

    public float pulseCooldown = .1f;


    private ContactFilter2D healableFilter;
    private bool pulseCharging = false;
    private bool pulseCapped = false;
    

    // Start is called before the first frame update
    void Start()
    {
        playerBase = GetComponent<PlayerBase>();
        supplyManager = GetComponent<PlayerSupplyManager>();

        healableFilter.SetLayerMask(healableLayers);
        pulseRing.localScale = new Vector3(pulseRadius * 2, pulseRadius * 2, 1);
        mesh = new Mesh();
        meshFilter.mesh = mesh;
    }

    void Update()
    {
        if(pulseCharging)
        {
            timeSinceChargeStart += Time.deltaTime;
            float percentChargeTimePass = Mathf.Clamp(timeSinceChargeStart / maxChargeTime, 0, 1);
            if(percentChargeTimePass * maxHealAmount > healFromCurrentCharge)
            {
                if(supplyManager.supplies >= (healFromCurrentCharge + healPerStep) * suppliesPerHeal)
                {
                    suppliesUsedByCharge = (healFromCurrentCharge + healPerStep) * suppliesPerHeal;
                    healFromCurrentCharge += healPerStep;
                    pulseCapped = false;
                }
                else
                {
                    pulseCapped = true; ;
                }
            }


            if(pulseCapped)
                GenerateCircle(Mathf.Clamp(healFromCurrentCharge / maxHealAmount, 0, 1));
            else
                GenerateCircle(percentChargeTimePass);
        }
        else
        {
            timeSincePulse += Time.deltaTime;
        }
    }
    // Update is called once per frame
    public void HealingPulse(CallbackContext ctx)
    {
        if(ctx.started && timeSincePulse > pulseCooldown && supplyManager.supplies >= suppliesPerHeal*healPerStep)
        {
            pulseRing.gameObject.SetActive(true);
            pulseCharging = true;
            timeSinceChargeStart = 0;
            healFromCurrentCharge = 0;
        }
        if(ctx.canceled && pulseCharging == true)
        {
            supplyManager.UseSupplies(suppliesUsedByCharge);
            pulseCharging = false;
            timeSincePulse = 0;
            Collider2D[] healingTargets = new Collider2D[20];
            int numHealingTargets = Physics2D.OverlapCircle(transform.position, pulseRadius, healableFilter, healingTargets);
            for (int i = 0; i < numHealingTargets; i++)
            {
                PlayerBase healedPlayerBase = healingTargets[i].gameObject.GetComponent<PlayerBase>();
                if(healedPlayerBase != null)
                {
                    healedPlayerBase.ChangeHealth(healFromCurrentCharge);
                }
            }

            mesh.Clear();
            StartCoroutine(GrowPulse());
        }
    }
    IEnumerator GrowPulse()
    {
        float circleDiameter = 0;
        while(circleDiameter <= pulseRadius * 2)
        {
            circleDiameter = (timeSincePulse / timeToGrowPulse) * (pulseRadius * 2);
            pulseField.localScale = new Vector3(circleDiameter, circleDiameter,1);
            yield return null;
        }
        pulseField.localScale = new Vector3(pulseRadius * 2, pulseRadius * 2, 1);
        yield return new WaitForSeconds(.1f);
        pulseField.localScale = new Vector3(0,0, 1);
        pulseRing.gameObject.SetActive(false);
    }

    void GenerateCircle(float percentComplete)
    {
        int stepsPerRotation = 100;
        float anglePerStep = 360/(float)stepsPerRotation;
        float angle = 90 - transform.rotation.eulerAngles.z;
        int steps = (int)(stepsPerRotation*percentComplete)+1;

        if (steps <= 2) //return if the shape is too small;
        {
            //Debug.Log("Bail due to steps count being too low");
            return;
        }

        List<Vector3> circlePoints = new List<Vector3>();
        for (int i = 0; i < steps; i++)
        {
            var dir = DirFromAngle(angle, true);
            circlePoints.Add(transform.position + dir * pulseRadius/2);
            angle -= anglePerStep;
        }
        

        int vertexCount = circlePoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;


        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(circlePoints[i]);

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
    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (angleIsGlobal != false)
        {
            angleInDegrees += transform.eulerAngles.z;  //if angle isnt in global space, convert it to be in global space.
        }
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }
}
