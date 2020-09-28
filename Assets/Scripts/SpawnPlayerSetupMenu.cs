using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerSetupMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject playerSetupPanelPrefab;
    public PlayerInput input;
    private void Awake()
    {
        var rootMenu = GameObject.Find("Main Layout");
        if(rootMenu != null)
        {
            var menu = Instantiate(playerSetupPanelPrefab,rootMenu.transform);
            input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            menu.GetComponent<PlayerSetupMenuController>().SetPlayerIndex(input.playerIndex);
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
