using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectSetup : MonoBehaviour
{
    [SerializeField]
    private GameObject levelButton;
    [SerializeField]
    private Transform contentField;
    private List<Button> LevelButtons = new List<Button>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(LevelData level in GameManager.Instance.levelData)
        {
            GameObject newLevelButton = Instantiate(levelButton, contentField);
            newLevelButton.GetComponentInChildren<TextMeshProUGUI>().SetText(level.displayName);
            newLevelButton.GetComponentInChildren<Button>().onClick.AddListener(() => SelectLevel(level.levelName));
            LevelButtons.Add(newLevelButton.GetComponentInChildren<Button>());
        }
        LevelButtons[0].Select();
    }

    // Update is called once per frame
    void SelectLevel(string LevelName)
    {
        GameManager.Instance.nextLevel = LevelName;
        SceneManager.LoadScene("PlayerSelect");
    }
}
