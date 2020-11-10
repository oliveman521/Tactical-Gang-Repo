using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumberBehavior : MonoBehaviour
{
    public float moveSpeed = 1;
    private float timeSinceSpawn = 0;
    public float timeTilFade = 1;
    public float timeTilDisapear = 5f;
    private float fadePerSec;
    public float xSpawnVariability;
    private TextMeshPro textMesh;
    private Color textColor;

    public Color healthGained;
    public Color healthLost;
    public Color ammoLost;
    public Color ammoGained;
    // Start is called before the first frame update
    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        textColor = textMesh.color;
        fadePerSec = 1 / (timeTilDisapear);
    }

    public void Setup(float changeAmount, Vector3 location, bool isHealth)
    {
        //set starting location
        location.x += Random.Range(-xSpawnVariability / 2, xSpawnVariability / 2);
        transform.position = location;

        //set color and text
        string text = "";
        if (changeAmount > 0) //if we gained
        {
            text = "+" + changeAmount.ToString();
            if (isHealth)
                textColor = healthGained;
            else
                textColor = ammoGained;
        }
        else if (changeAmount < 0) //if we lost
        {
            text = "-" + Mathf.Abs(changeAmount).ToString();
            if (isHealth)
                textColor = healthLost;
            else
                textColor = ammoLost;
        }
        else
        {
            Debug.Log("Error, damage done was 0, so damage number shouldn't've been spawned");
            Destroy(this.gameObject);
        }
        textMesh.color = textColor;
        textMesh.SetText(text);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceSpawn += Time.deltaTime;
        transform.position += Vector3.up * Time.deltaTime * moveSpeed;

        if(timeSinceSpawn >= timeTilFade)
        {
            textColor.a -= fadePerSec * Time.deltaTime; //fade out the damage number (alpha is measuered from 0-1 in this case for some reason)
            textMesh.color = textColor;
            if(textColor.a < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
