using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private GameObject levelFailedUI;

    private bool isPaused = false;



    private void Awake()
    {
        // Ensure there's only one instance of GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist through scene changes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Function to complete the level
    public void CompleteLevel()
    {
        levelCompleteUI?.SetActive(true);
        Time.timeScale = 0f;
    }

    // Function to fail the level
    public void FailLevel()
    {
        levelFailedUI?.SetActive(true);
        Time.timeScale = 0f;
    }

    // Function to pause the game
    public void PauseGame()
    {
        if (!isPaused)
        {
            pauseMenuUI?.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    // Function to resume the game
    public void ResumeGame()
    {
        if (isPaused)
        {
            pauseMenuUI?.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    // Function to restart the level
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f; // Ensure the game is running normally
        SceneManager.LoadScene(sceneName);
    }

   
}
