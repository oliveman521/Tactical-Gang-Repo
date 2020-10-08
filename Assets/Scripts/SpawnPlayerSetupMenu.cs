using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerSetupMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject playerSetupMenuPrefab;
    private void Awake()
    {
        var rootMenu = GameObject.Find("Main Layout");
        if (rootMenu != null)
        {
            foreach (PlayerConfiguration playerConfig in GameManager.Instance.playerConfigs)
            {
                var menu = Instantiate(playerSetupMenuPrefab, rootMenu.transform);
                playerConfig.playerInput.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
                menu.GetComponent<PlayerSetupMenuController>().SetPlayerIndex(playerConfig.playerInput.playerIndex);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
