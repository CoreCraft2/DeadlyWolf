using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Function to load a scene by its name
    public void LoadScene(string sceneName)
    {
        // Check if the scene name is valid and loaded in the build settings
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene " + sceneName + " cannot be loaded. Make sure it is added to the build settings.");
        }
    }
}
