using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float FPS;

    public List<PlayerConfiguration> playerConfigs;

    [HideInInspector] public GameObject selectOnJoin;
    [HideInInspector] public static GameManager Instance { get; private set; }
    [HideInInspector] public PlayerInputManager inputManager;


    [Header("Data")]
    public Color[] mercColorDictionary;
    public MercData[] mercData;
    public Dictionary<MercTag, MercData> mercDictionary = new Dictionary<MercTag, MercData>();
    public LevelData[] levelData;

    [HideInInspector] public bool gameIsPaused = false;
    [HideInInspector]public string nextLevel;

    private void Awake()
    {
        //Creating the singleton of this script (with basic check to make sure it doesn't already exist)
        if(Instance != null)
        {
            Debug.Log("SINGLETON - Trying to create another instance - error");
            Destroy(this.gameObject);
        }
        else
        {   //creating a singleton
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }

        //setup the merc data dictionary
        foreach (MercData mercDataPoint in mercData)
        {
            mercDictionary.Add(mercDataPoint.mercTag, mercDataPoint);
        }
        inputManager = GetComponent<PlayerInputManager>();
        //subscribe to find out when new scenes are loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
        inputManager.onPlayerJoined += OnPlayerJoined;
        inputManager.onPlayerLeft += OnPlayerLeft;
    }

    void Update()
    {
        FPS = (1f / Time.unscaledDeltaTime);
    }
    public void SetMerc(int playerIndex, MercTag mercTag)
    {
        playerConfigs[playerIndex].merc = mercTag;
    }
    public void SetPlayerColor(int playerIndex, int colorIndex)  //setting the color of our new player
    {
        playerConfigs[playerIndex].color = mercColorDictionary[colorIndex];
    }
    public void ReadyPlayer(int index)   //readying up the player TODO: unreadying
    {
        playerConfigs[index].isReady = true;

        int readyPlayers = 0;
        foreach (PlayerConfiguration playerConfig in playerConfigs)
        {
            if (playerConfig.isReady)
                readyPlayers++;
        }
        if(readyPlayers == playerConfigs.Count)
        {
            SceneManager.LoadScene(nextLevel);
            foreach (PlayerConfiguration playerConfig in playerConfigs) //reset the ready state for each playerConfig.
            {
                playerConfig.isReady = false;
            }
        }
    }
    public void OnPlayerJoined(PlayerInput pi)
    {
        pi.transform.SetParent(transform); //Todo: rework player joining so that there isn't an extra player configuration added when player spawns in

        //check if the player is new. Make sure their index isnt already saved in playerConfigs
        PlayerConfiguration existingConfig = null;
        if (playerConfigs.Count > 0)
        {
            foreach (PlayerConfiguration playerConfig in playerConfigs)
            {
                if (playerConfig.index == pi.playerIndex)
                {
                    existingConfig = playerConfig;
                }
            }
        }
        if (existingConfig == null) //the config in question is new, not in the list yet. Spawn in some shit
        {
            PlayerConfiguration newPlayerConfig = new PlayerConfiguration(pi);
            playerConfigs.Add(newPlayerConfig);
            if(selectOnJoin)
                newPlayerConfig.mpEventSystem.SetSelectedGameObject(selectOnJoin);
            Debug.Log("Player " + pi.playerIndex + " Joined!!");
                 
        }
        else //the config in question exists already, just set it back to connected status
        {
            Debug.Log("New player was already in the game at some point, but still had a config sitting around? Error!");
        }
    }
    public void OnPlayerLeft(PlayerInput pi)
    {
        PlayerConfiguration disconnectedConfig = null;
        foreach (PlayerConfiguration playerConfig in playerConfigs)
        {
            if(playerConfig.index == pi.playerIndex)
            {
                disconnectedConfig = playerConfig;
            }
        }
        if(disconnectedConfig != null)
        {
            playerConfigs.Remove(disconnectedConfig);
            Debug.Log("player suggessfully removed");
        }
        else
        {
            Debug.Log("Error - Player who disconnected could not be found");
        }

    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }


}

[Serializable]
public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        index = pi.playerIndex;
        playerInput = pi;
        mpEventSystem = pi.gameObject.GetComponentInChildren<MultiplayerEventSystem>();
    }
    public PlayerInput playerInput { get; set; }
    public MultiplayerEventSystem mpEventSystem;
    public int index { get; set; }
    public bool isReady { get; set; }
    public Color color { get; set; }
    public MercTag merc { get; set; }
}

[Serializable]
public class MercData
{
    public MercTag mercTag;
    public GameObject mercPrefab;
    public Sprite mercIcon;
}

[Serializable]
public class LevelData
{
    public string levelName;
    public string displayName;
    public Sprite icon;
}
public enum MercTag
{
    Gunner,
    Sniper,
    Demoman,
    Mule,
    Rat,
    Ops,
    Turtle, 
    Medic,
}