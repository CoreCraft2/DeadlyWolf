using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button[] levelButtons;

    void Start()
    {
        // Load unlocked level information
        int unlockedLevel = PlayerPrefs.GetInt("unlockedLevel", 1);

        // Set buttons interactable based on unlock status
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > unlockedLevel)
            {
                levelButtons[i].interactable = false; // Lock the level
            }
        }
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void UnlockNextLevel(int levelIndex)
    {
        int unlockedLevel = PlayerPrefs.GetInt("unlockedLevel", 1);
        if (levelIndex >= unlockedLevel)
        {
            PlayerPrefs.SetInt("unlockedLevel", levelIndex + 1);
        }
    }
}
