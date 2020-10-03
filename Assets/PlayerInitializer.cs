using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInitializer : MonoBehaviour
{
    private List<Transform> availableSpawnPoints = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        //generate list of spawnpoints
        GameObject[] spawnPointsObjs = GameObject.FindGameObjectsWithTag("Spawn Point");
        foreach (GameObject spawnPointObj in spawnPointsObjs)
        {
            availableSpawnPoints.Add(spawnPointObj.GetComponent<Transform>());
        }


        //spawn all players
        if (PlayerConfigurationManager.Instance)
        {
            PlayerConfiguration[] playerConfigs = PlayerConfigurationManager.Instance.playerConfigs.ToArray();
            foreach (PlayerConfiguration pc in playerConfigs)
            {
                Transform spawnPoint = PickSpawnPoint();
                PlayerConfigurationManager.Instance.mercDictionary.TryGetValue(pc.merc, out MercData mercData);
                if (mercData == null)
                {
                    Debug.Log("Merc not in dictionary");
                }
                else
                {
                    //spawn player
                    GameObject newPlayerObj = Instantiate(mercData.mercPrefab, spawnPoint.position, spawnPoint.rotation);

                    //set their color
                    newPlayerObj.GetComponent<PlayerBase>().playerColorOverlay.color = pc.color;


                    //configure controls from the player's prefab
                    pc.playerInput.actionEvents = newPlayerObj.GetComponent<PlayerInput>().actionEvents; //steal all of the action mapping from the player object
                    Destroy(newPlayerObj.GetComponent<PlayerInput>()); //delete the playerinput object from the player
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    Transform PickSpawnPoint()
    {
        int index = Random.Range(0, availableSpawnPoints.Count);
        Transform chosenPoint = availableSpawnPoints[index];
        availableSpawnPoints.RemoveAt(index);
        return chosenPoint;
    }

}
