using UnityEngine;
using System.Collections;

public class StoneManager : MonoBehaviour
{
    public static StoneManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ScheduleRespawn(Stone stone, float delay)
    {
        StartCoroutine(RespawnCoroutine(stone, delay));
    }

    public void ScheduleDelayedCompletion(float delay, Stone stone)
    {
        StartCoroutine(DelayedCompletionCoroutine(delay, stone));
    }

    private IEnumerator RespawnCoroutine(Stone stone, float delay)
    {
        yield return new WaitForSeconds(delay);
        stone.Respawn();
    }

    private IEnumerator DelayedCompletionCoroutine(float delay, Stone stone)
    {
        yield return new WaitForSeconds(delay);

        // Only complete the level if the stone is active
        if (stone != null && stone.gameObject.activeInHierarchy)
        {
            if (!stone.wolfAnimator.GetBool("isDied"))
            {
                GameManager.Instance.CompleteLevel(); // Complete level after delay if not already completed
            }
        }
    }
}
