using UnityEngine;
using UnityEngine.UI; // Import for UI components
using System.Collections;

public class Stone : MonoBehaviour
{
    public Animator wolfAnimator; // Reference to the wolf's Animator
    public ParticleSystem smokeEffect; // Reference to the smoke particle system
    public CameraShake cameraShake; // Reference to the CameraShake script
    public LayerMask groundLayer; // Assign the ground layer in the inspector
    public Transform startPosition; // Reference to the start position for respawn
    public float respawnDelay = 1f; // Delay before respawning in seconds
    public int maxAttempts = 3; // Maximum number of attempts
    public Text attemptsText; // Reference to the UI Text component for displaying attempts

    private bool hasCollidedWithWolf = false;
    private bool hasCollidedWithGround = false;
    private int attemptsLeft;

    private void Start()
    {
        // Initialize the number of attempts
        attemptsLeft = maxAttempts;

        // Ensure that the smoke effect is not playing at start
        if (smokeEffect != null)
        {
            smokeEffect.Stop();
        }

        // Update the UI with the initial number of attempts
        UpdateAttemptsUI();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wolf") && !hasCollidedWithWolf)
        {
            hasCollidedWithWolf = true;
            HandleWolfCollision();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !hasCollidedWithGround)
        {
            hasCollidedWithGround = true;
            HandleGroundCollision();
        }
    }

    private void HandleWolfCollision()
    {
        if (wolfAnimator != null)
        {
            wolfAnimator.SetBool("isDied", true);
            GameManager.Instance.CompleteLevel();
        }

        if (smokeEffect != null)
        {
            smokeEffect.Play();
        }

        if (cameraShake != null)
        {
            cameraShake.ShakeCamera();
        }

        if (attemptsLeft <= 1) // If it's the last attempt
        {
            GameManager.Instance.CompleteLevel();
        }
        else
        {
            DeactivateStone();
            StartCoroutine(DelayedCompletion(2f)); // Wait for 2 seconds before completing the level
        }
    }

    private void HandleGroundCollision()
    {
        attemptsLeft--;

        // Update the UI with the new number of attempts
        UpdateAttemptsUI();

        if (attemptsLeft > 0)
        {
            DeactivateStone();
            StoneManager.Instance.ScheduleRespawn(this, respawnDelay); // Schedule respawn
        }
        else if(attemptsLeft == 0)
        {
            GameManager.Instance.FailLevel();
           // DeactivateStone(); // Optionally deactivate the stone if out of attempts
        }
    }

    private IEnumerator DelayedCompletion(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!wolfAnimator.GetBool("isDied"))
        {
            GameManager.Instance.CompleteLevel(); // Complete level after delay if not already completed
        }
    }

    private void DeactivateStone()
    {
        gameObject.SetActive(false);
    }

    public void Respawn()
    {
        if (startPosition != null)
        {
            transform.position = startPosition.position;
            transform.rotation = startPosition.rotation;
            gameObject.SetActive(true); // Reactivate the stone
            gameObject.GetComponent<Rigidbody>().useGravity = false;

            // Reset collision flags
            hasCollidedWithWolf = false;
            hasCollidedWithGround = false;
        }
        else
        {
            Debug.LogError("Start Position is not assigned.");
        }
    }

    private void UpdateAttemptsUI()
    {
        if (attemptsText != null)
        {
            attemptsText.text = "Hits: " + attemptsLeft;
        }
        else
        {
            Debug.LogError("UI Text component is not assigned.");
        }
    }
}
