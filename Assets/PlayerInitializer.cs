using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        PlayerConfiguration[] playerConfigs = PlayerConfigurationManager.Instance.playerConfigs.ToArray();
        foreach(PlayerConfiguration pc in playerConfigs)
        {
            Transform spawnPoint = PickSpawnPoint();
            PlayerConfigurationManager.Instance.mercDictionary.TryGetValue(pc.merc, out MercData mercData);
            if(mercData == null)
            {
                Debug.Log("Merc not in dictionary");
            }
            else
            {
                GameObject newPlayerObj = Instantiate(mercData.mercPrefab, spawnPoint.position, spawnPoint.rotation);
                newPlayerObj.GetComponent<PlayerInput>().actions = pc.playerInput.actions;
                newPlayerObj.GetComponent<SpriteRenderer>().color = pc.color;
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
