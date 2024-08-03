using UnityEngine;

public class DragAndShoot3D : MonoBehaviour
{
    public GameObject ballPrefab; // Prefab of the ball
    public Transform shootPoint; // Point from where the ball is shot
    public float shootForce = 10f; // Force applied to the ball

    private Vector3 startPos;
    private Camera cam;
    private GameObject currentBall;
    private Rigidbody ballRb;
    private bool isDragging = false;

    void Start()
    {
        cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("Main Camera not found.");
        }

        if (ballPrefab == null)
        {
            Debug.LogError("Ball prefab is not assigned.");
        }

        if (shootPoint == null)
        {
            Debug.LogError("Shoot point is not assigned.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnDragStart();
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            OnDrag();
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            OnDragEnd();
        }
    }

    void OnDragStart()
    {
        startPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane + 10f));
        isDragging = true; // Set dragging state to true
    }

    void OnDrag()
    {
        // Optionally, you can implement visual feedback during dragging
    }

    void OnDragEnd()
    {
        if (!isDragging) return;

        Vector3 endPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane + 10f));
        Vector3 direction = startPos - endPos;

        // Instantiate the ball at the shootPoint
        currentBall = Instantiate(ballPrefab, shootPoint.position, Quaternion.identity);
        ballRb = currentBall.GetComponent<Rigidbody>();

        if (ballRb == null)
        {
            Debug.LogError("Rigidbody component missing on currentBall.");
            return;
        }

        // Apply force to the ball in the direction of the drag
        ballRb.velocity = direction * shootForce;

        // Optionally destroy the ball after some time
        Destroy(currentBall, 2f); // Destroy ball after 2 seconds

        currentBall = null;
        ballRb = null;
        isDragging = false; // Set dragging state to false
    }
}
