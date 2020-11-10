using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("PauseMenuSettings")]
    public GameObject PausePanel;
    public GameObject selectedBeforePause;
    private PlayerInput pi;
    public MultiplayerEventSystem mpEventSystem;
    // Start is called before the first frame update
    void Start()
    {
        PausePanel.SetActive(false); //make sure the pause menu is off
        pi = GetComponentInParent<PlayerInput>();
        AttatchPauseAction();
    }
    public void AttatchPauseAction()
    {
        foreach (PlayerInput.ActionEvent actionEvent in pi.actionEvents)
        {
            if (actionEvent.actionName.Contains("Menu"))
            {
                UnityAction<InputAction.CallbackContext> pauseAction = Pause;
                actionEvent.AddListener(pauseAction);
            }
        }
    }
    public void Pause(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (GameManager.Instance.gameIsPaused == false)
            {
                GameManager.Instance.gameIsPaused = true;
                selectedBeforePause = mpEventSystem.currentSelectedGameObject;
                Time.timeScale = 0;
                PausePanel.SetActive(true);
                List<Button> buttonsInPauseMenu = PausePanel.GetComponentsInChildren<Button>().ToList<Button>();
                mpEventSystem.SetSelectedGameObject(null);
                mpEventSystem.SetSelectedGameObject(buttonsInPauseMenu[0].gameObject);
            }
            else if (GameManager.Instance.gameIsPaused == true && PausePanel.activeSelf == true)
            {
                Resume();
            }
        }
    }
    public void Resume()
    {
        GameManager.Instance.gameIsPaused = false;
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        mpEventSystem.SetSelectedGameObject(null);
        mpEventSystem.SetSelectedGameObject(selectedBeforePause);
    }
    public void RestartLevel()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LoadScene(string sceneName)
    {
        Resume();
        SceneManager.LoadScene(sceneName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }


}
