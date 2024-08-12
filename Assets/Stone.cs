using UnityEngine;
using UnityEngine.UI;

public class Stone : MonoBehaviour
{
    public Animator wolfAnimator;
    public ParticleSystem smokeEffect;
    public CameraShake cameraShake;
    public LayerMask groundLayer;
    public Transform startPosition;
    public float respawnDelay = 1f;
    public int maxAttempts = 3;
    public Text attemptsText;

    private bool hasCollidedWithWolf = false;
    private bool hasCollidedWithGround = false;
    private int attemptsLeft;

    private void Start()
    {
        attemptsLeft = maxAttempts;

        if (smokeEffect != null)
        {
            smokeEffect.Stop();
        }

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

        if (attemptsLeft <= 1)
        {
            GameManager.Instance.CompleteLevel();
        }
        else
        {
            DeactivateStone();
            StoneManager.Instance.ScheduleDelayedCompletion(2f, this); // Request delayed completion
        }
    }

    private void HandleGroundCollision()
    {
        attemptsLeft--;

        UpdateAttemptsUI();

        if (attemptsLeft > 0)
        {
            DeactivateStone();
            StoneManager.Instance.ScheduleRespawn(this, respawnDelay);
        }
        else if (attemptsLeft == 0)
        {
            GameManager.Instance.FailLevel();
<<<<<<< Updated upstream
            // DeactivateStone(); // Optionally deactivate the stone if out of attempts
        }
    }

    private IEnumerator DelayedCompletion(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!wolfAnimator.GetBool("isDied"))
        {
            GameManager.Instance.CompleteLevel(); // Complete level after delay if not already completed
=======
>>>>>>> Stashed changes
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
            gameObject.SetActive(true);
            gameObject.GetComponent<Rigidbody>().useGravity = false;

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
