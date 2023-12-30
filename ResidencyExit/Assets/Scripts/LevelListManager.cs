using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelListManager : MonoBehaviour
{
    [SerializeField] Transform content;
    [SerializeField] GameObject levelButtonPrefab;
    [SerializeField] LevelDataScriptableObject levelData;
    [SerializeField] GameDataScriptableObject gameDataSO;

    private List<LevelButton> levelButtons;

    private void Awake()
    {
        levelButtons = new List<LevelButton>();
    }

    private void Start()
    {
        for(int i = 0;i < levelData.levelDataCSV.Length;i++)
        {
            var temp = Instantiate(levelButtonPrefab);
            temp.transform.SetParent(content.transform, false);
            temp.GetComponentInChildren<TMPro.TMP_Text>().text = (i + 1).ToString();
            Button button = temp.GetComponent<Button>();
            LevelButton levelButton = new  LevelButton(button, i);
            button.onClick.AddListener(delegate { LoadLevel(levelButton.levelId);});
            button.interactable = false;
            levelButtons.Add(levelButton);
        }

        for (int i = 0; i < gameDataSO.completedLevels + 1; i++)
        {
            levelButtons[i].button.interactable = true;
        }
        
    }


    public void LoadLevel(int levelId)
    {
        gameDataSO.currentLevel = levelId;
        SceneManager.LoadScene("GamePlay");
    }
}
public struct LevelButton
{
    public LevelButton(Button button,int levelId)
    {
        this.button = button;
        this.levelId = levelId;
    }
    public Button button;
    public int levelId;
}

