using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelectMenuInitializer : MonoBehaviour
{
    [SerializeField] private GameObject playerSetupMenuPrefab = null;
    // Start is called before the first frame update
    void Start()
    {
        foreach (PlayerConfiguration playerConfig in GameManager.Instance.playerConfigs)
        {
            SpawnPlayerSelectMenu(playerConfig.playerInput);
        }
        GameManager.Instance.inputManager.onPlayerJoined += SpawnPlayerSelectMenu;
        SceneManager.sceneUnloaded += sceneUnloaded;
    }

    private void sceneUnloaded(Scene arg0)
    {
        GameManager.Instance.inputManager.onPlayerJoined -= SpawnPlayerSelectMenu;
        SceneManager.sceneUnloaded -= sceneUnloaded;
    }

    public void SpawnPlayerSelectMenu(PlayerInput pi)
    {
        var rootMenu = GameObject.Find("Player Select UI Layout");
        if (rootMenu != null)
        {
            var menu = Instantiate(playerSetupMenuPrefab, rootMenu.transform);
            menu.GetComponent<PlayerSetupMenuController>().SetPlayerIndex(pi.playerIndex);
            menu.transform.SetSiblingIndex(menu.transform.parent.childCount - 2);
        }
    }

}
