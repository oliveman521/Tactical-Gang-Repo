using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int playerIndex;

    [SerializeField]
    private TextMeshProUGUI titleText = null;
    [SerializeField]
    private Button readyButton = null;
    [SerializeField]
    private GameObject readyConfirmation = null;
    [SerializeField]
    private GameObject colorButtonPrefab = null;
    [SerializeField]
    private GameObject colorButtonLayoutGroup = null;
    [SerializeField]
    private GameObject mercButtonPrefab = null;
    [SerializeField]
    private GameObject mercButtonLayoutGroup = null;

    private MultiplayerEventSystem mpEventSystem;

    private float ignoreInputTime = .5f;
    private bool inputEnabled;
    private List<GameObject> colorButtonsList = new List<GameObject>();
    private List<GameObject> mercButtonsList = new List<GameObject>();

    public GameObject[] playerSetupMenus;
    private int currentMenu = 0;
    private GameObject lastSelectedObject;

    public void SetPlayerIndex(int pi)
    {
        playerIndex = pi;
        titleText.SetText("Player " + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    private void Start()
    {
        mpEventSystem = GameManager.Instance.playerConfigs[playerIndex].mpEventSystem;

        //uiInputModule.cancel.action.actionMap.AddAction(() => PastMenu())
        GenerateColorSelectField();
        GenerateMercSelectField();
        playerSetupMenus[0].SetActive(true);
        //generate a list of buttons in the first menu, then select the first one;
        Button[] buttonsInNextMenu = playerSetupMenus[currentMenu].GetComponentsInChildren<Button>();
        mpEventSystem.SetSelectedGameObject(null);
        mpEventSystem.SetSelectedGameObject(buttonsInNextMenu[0].gameObject);
    }
    void Update()
    {
        if(Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
        UpdateOutline();
    }
    public void NextMenu()
    {
        try
        {
            GameObject upcomingMenu = playerSetupMenus[currentMenu + 1];
        }
        catch
        {
            Debug.Log("The Menu you are trying to reach doesnt exist");
            return;
        }
        playerSetupMenus[currentMenu].SetActive(false);
        currentMenu+=1;
        playerSetupMenus[currentMenu].SetActive(true);
        Button[] buttonsInNextMenu = playerSetupMenus[currentMenu].GetComponentsInChildren<Button>();
        mpEventSystem.SetSelectedGameObject(null);
        mpEventSystem.SetSelectedGameObject(buttonsInNextMenu[0].gameObject);
    }
    public void PastMenu()
    {
        Debug.Log("Past Menu triggered");
        try
        {
            GameObject upcomingMenu = playerSetupMenus[currentMenu - 1];
        }
        catch
        {
            Debug.Log("The Menu you are trying to reach doesnt exist");
            return;
        }
        playerSetupMenus[currentMenu].SetActive(false);
        currentMenu -= 1;
        playerSetupMenus[currentMenu].SetActive(true);
        Button[] buttonsInNextMenu = playerSetupMenus[currentMenu].GetComponentsInChildren<Button>();
        buttonsInNextMenu[0].Select();
    }
    public void SetMerc(MercTag mercTag)
    {
        if (!inputEnabled)
            return;
        GameManager.Instance.SetMerc(playerIndex, mercTag);
        NextMenu();
    }
    public void SetColor(int colorIndex)
    {
        if (!inputEnabled)
            return;
        GameManager.Instance.SetPlayerColor(playerIndex, colorIndex);
        NextMenu();
    }
    public void UpdateOutline()
    {
        if (mpEventSystem.currentSelectedGameObject != lastSelectedObject)
        {
            Outline outline = mpEventSystem.currentSelectedGameObject.GetComponent<Outline>();
            if(outline)
                 outline.enabled = true;
            if (lastSelectedObject != null)
            {
                Outline lastOutline = lastSelectedObject.GetComponent<Outline>();
                if(lastOutline)
                    lastOutline.enabled = false;
            }

        }
        lastSelectedObject = mpEventSystem.currentSelectedGameObject;
    }
    public void ReadyPlayer()
    {
        if (!inputEnabled)
            return;
        GameManager.Instance.ReadyPlayer(playerIndex);
        readyButton.gameObject.SetActive(false);
        readyConfirmation.SetActive(true);
        readyConfirmation.GetComponent<TextMeshProUGUI>().SetText("Player " + (playerIndex+1).ToString() + " is Ready.");
    }
    public void GenerateColorSelectField()
    {
        for (int i = 0; i < GameManager.Instance.mercColorDictionary.Length; i++)
        {
            int currentIndex = i;
            colorButtonsList.Add(Instantiate(colorButtonPrefab, colorButtonLayoutGroup.transform));
            colorButtonsList[i].GetComponent<Image>().color = GameManager.Instance.mercColorDictionary[i];
            Button newButton = colorButtonsList[i].GetComponent<Button>();
            newButton.onClick.AddListener(() => SetColor(currentIndex));
        }
    }
    public void GenerateMercSelectField()
    {
        string[] mercTagStrings = System.Enum.GetNames(typeof(MercTag));
        for (int i = 0; i < mercTagStrings.Length; i++)
        {
            mercButtonsList.Add(Instantiate(mercButtonPrefab, mercButtonLayoutGroup.transform));
            mercButtonsList[i].GetComponentInChildren<TextMeshProUGUI>().SetText(mercTagStrings[i]);
            Button newButton = mercButtonsList[i].GetComponent<Button>();
            MercTag currentMerctag = (MercTag)System.Enum.Parse(typeof(MercTag), mercTagStrings[i]);
            newButton.onClick.AddListener(() => SetMerc(currentMerctag));
        }
    }

}
