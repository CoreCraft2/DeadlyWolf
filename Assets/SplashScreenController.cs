using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenController : MonoBehaviour
{
    // Duration for the splash screen in seconds
    public float splashScreenDuration = 10f;

    private void Start()
    {
        // Start the coroutine to handle splash screen timing
        StartCoroutine(ShowSplashScreen());
    }

    private IEnumerator ShowSplashScreen()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(splashScreenDuration);

        // Load the MainMenu scene
        SceneManager.LoadScene("MainMenu");
    }
}
