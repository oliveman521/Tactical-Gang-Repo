using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    public float FPS;
    public List<PlayerConfiguration> playerConfigs;
    public static PlayerConfigurationManager Instance { get; private set; }
    public Color[] colorDictionary;
    public MercData[] mercData;
    public Dictionary<MercTag, MercData> mercDictionary = new Dictionary<MercTag, MercData>();

    public string nextLevel;

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
        playerConfigs[playerIndex].color = colorDictionary[colorIndex];
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