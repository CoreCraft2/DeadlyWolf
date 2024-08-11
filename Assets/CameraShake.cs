using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f; // Duration of the shake
    public float shakeMagnitude = 0.1f; // Magnitude of the shake
    private Vector3 originalPosition; // Original position of the camera
    private float shakeTimer = 0f; // Timer to track the shake duration

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            // Apply a random shake to the camera position
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;

            // Decrease the shake timer
            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0)
            {
                // Reset camera position and shake parameters
                transform.localPosition = originalPosition;
                shakeTimer = 0;
            }
        }
    }

    public void ShakeCamera()
    {
        shakeTimer = shakeDuration;
    }
}
