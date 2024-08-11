using UnityEngine;

public class WolfMove : MonoBehaviour
{
    public Transform pointA; // The first point
    public Transform pointB; // The second point
    public float speed = 2.0f; // Speed at which the wolf moves

    private Animator animator;
    private Transform targetPoint;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isWalking", true); // Start the walking animation
        targetPoint = pointB; // Set the initial target point
    }

    void Update()
    {
        // Check if the wolf is dead
        if (animator != null && animator.GetBool("isDied"))
        {
            // Stop movement if the wolf is dead
            animator.SetBool("isWalking", false);
            return;
        }

        // Continue movement if the wolf is not dead
        MoveBetweenPoints();
    }

    private void MoveBetweenPoints()
    {
        // Move the wolf towards the target point
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // If the wolf reaches the target point, switch to the other point
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            targetPoint = targetPoint == pointA ? pointB : pointA;

            // Rotate the wolf to face the new target point
            transform.LookAt(targetPoint);
        }
    }
}
