using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator = null;  // Reference to the Animator component
    private const string AIM = "Aim";
    private const string SHOOT = "Shoot";
    private bool isDragging = false;

    public GameObject ArrowPrefab; // Reference to the arrow prefab
    public Transform ArrowPoint; // Position from which the arrow is shot
    public LineRenderer lineRenderer; // Reference to the Line Renderer
    public float arrowForceMultiplier = 30f; // Force applied to the arrow
    public Vector3 rotationOffset; // Rotation offset to adjust arrow direction
    public float arrowLifeTime = 4f; // Time after which the arrow will be destroyed

    private GameObject currentArrow; // Reference to the currently instantiated arrow

    private Vector3 aimDirection;

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            animator.SetBool(AIM, true);
            lineRenderer.enabled = true; // Enable line renderer when aiming
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            UpdateAimDirection();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            animator.SetBool(AIM, false);
            animator.SetBool(SHOOT, true);
            lineRenderer.enabled = false; // Disable line renderer after shooting
            Shoot();
        }

        // Reset SHOOT parameter after a short time
        if (!isDragging && animator.GetBool(SHOOT))
        {
            StartCoroutine(ResetShootParameter());
        }
    }

    private void UpdateAimDirection()
    {
        // Update the aim direction based on the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            aimDirection = (hit.point - ArrowPoint.position).normalized;
            aimDirection.y = 0; // Keep the aim direction on the horizontal plane

            // Update the Line Renderer to show the direction
            lineRenderer.SetPosition(0, ArrowPoint.position);
            lineRenderer.SetPosition(1, hit.point); // End of the line at the hit point
        }
    }

    public void Shoot()
    {
        // Destroy the current arrow if it exists
        if (currentArrow != null)
        {
            Destroy(currentArrow);
        }

        // Calculate the direction based on the Line Renderer positions
        Vector3 shootDirection = (lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0)).normalized;

        // Instantiate the arrow at ArrowPoint's position with the correct rotation
        currentArrow = Instantiate(ArrowPrefab, ArrowPoint.position, Quaternion.Euler(130, -45, -40));

        // Apply any additional rotation offset (set in the Inspector)
        currentArrow.transform.rotation *= Quaternion.Euler(rotationOffset);

        Rigidbody arrowRb = currentArrow.GetComponent<Rigidbody>();

        if (arrowRb != null)
        {
            // Apply force to the arrow in the line renderer's direction
            Vector3 force = shootDirection * arrowForceMultiplier;
            arrowRb.AddForce(force, ForceMode.Impulse);

            // Destroy the arrow after the specified lifetime
            Destroy(currentArrow, arrowLifeTime);
        }
        else
        {
            Debug.LogError("Arrow does not have a Rigidbody component.");
        }
    }

    private IEnumerator ResetShootParameter()
    {
        yield return new WaitForSeconds(0.1f); // Adjust delay as needed
        animator.SetBool(SHOOT, false);
    }
}
