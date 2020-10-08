using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float FPS;
    public List<PlayerConfiguration> playerConfigs;
    public static GameManager Instance { get; private set; }
    public Color[] mercColorDictionary;
    public MercData[] mercData;
    public Dictionary<MercTag, MercData> mercDictionary = new Dictionary<MercTag, MercData>();
    public LevelData[] levelData;

    public string nextLevel;

    [SerializeField]
    private GameObject playerSetupMenuPrefab;

    private void Awake()
    {
        //setup the merc data dictionary
        foreach(MercData mercDataPoint in mercData)
        {
            mercDictionary.Add(mercDataPoint.mercTag, mercDataPoint);
        }

        //Creating the singleton of this script (with basic check)
        if(Instance != null)
        {
            Debug.Log("SINGLETON - Trying to create another instance - error");
        }
        else
        {   //creating a singleton
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }

        //subscribe to find out when new scenes are loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        Debug.Log("Player Joined!!" + pi.playerIndex);
        pi.transform.SetParent(transform);

        //check if the player is new. MAke sure their index isnt already saved in playerConfigs
        bool isNewPlayer = true;
        if (playerConfigs.Count > 0)
        {
            foreach (PlayerConfiguration playerConfig in playerConfigs)
            {
                if (playerConfig.index == pi.playerIndex)
                {
                    isNewPlayer = false;
                }
            }
        }
        if (isNewPlayer)
        {
            playerConfigs.Add(new PlayerConfiguration(pi));
            
            //set up a player setup menu for new player if necessary
            if (SceneManager.GetActiveScene().name == "PlayerSelect")
            {
                  SpawnPlayerSelectMenu(pi);
            }
        }
        
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "PlayerSelect")
        {
            foreach (PlayerConfiguration playerConfig in GameManager.Instance.playerConfigs)
            {
                SpawnPlayerSelectMenu(playerConfig.playerInput);
            }
        }
    }
    public void SpawnPlayerSelectMenu(PlayerInput pi)
    {
        var rootMenu = GameObject.Find("Player Select UI Layout");
        if (rootMenu != null)
        {
            var menu = Instantiate(playerSetupMenuPrefab, rootMenu.transform);
            pi.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            menu.GetComponent<PlayerSetupMenuController>().SetPlayerIndex(pi.playerIndex);
        }
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        index = pi.playerIndex;
        playerInput = pi;
    }
    public PlayerInput playerInput { get; set; }
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