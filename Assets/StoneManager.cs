using UnityEngine;
using System.Collections;

public class StoneManager : MonoBehaviour
{
    public static StoneManager Instance; // Singleton instance

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

    private IEnumerator RespawnCoroutine(Stone stone, float delay)
    {
        yield return new WaitForSeconds(delay);
        stone.Respawn();
    }
}
