using UnityEngine;

public class Stone : MonoBehaviour
{
    public Animator wolfAnimator; // Reference to the wolf's Animator
    public ParticleSystem smokeEffect; // Reference to the smoke particle system
    public CameraShake cameraShake; // Reference to the CameraShake script

    private bool hasCollidedWithWolf = false;

    private void Start()
    {
        // Ensure that the smoke effect is not playing at start
        if (smokeEffect != null)
        {
            smokeEffect.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the stone has collided with the wolf
        if (collision.gameObject.CompareTag("Wolf") && !hasCollidedWithWolf)
        {
            hasCollidedWithWolf = true;

            // Trigger the wolf's die animation
            if (wolfAnimator != null)
            {
                wolfAnimator.SetBool("isDied", true);
            }
            else
            {
                Debug.LogError("Wolf Animator is not assigned.");
            }

            // Play the smoke particle effect
            if (smokeEffect != null)
            {
                smokeEffect.Play();
            }
            else
            {
                Debug.LogError("Smoke Particle System is not assigned.");
            }

            // Trigger camera shake
            if (cameraShake != null)
            {
                cameraShake.ShakeCamera();
            }
            else
            {
                Debug.LogError("CameraShake script is not assigned.");
            }

            // Destroy the stone after the particle effect duration
            Destroy(gameObject, smokeEffect.main.duration);
        }
    }
}
