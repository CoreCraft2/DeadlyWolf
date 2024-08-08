using UnityEngine;

public class DragAndShoot3D : MonoBehaviour
{
    public GameObject ballPrefab; // Prefab of the ball
    public Transform shootPoint; // Point from where the ball is shot
    public float shootForce = 10f; // Force applied to the ball
    public LineRenderer trajectoryRenderer; // LineRenderer for showing trajectory

    private Vector3 startPos;
    private Vector3 endPos;
    private Camera cam;
    private GameObject mainBall;
    private bool isDragging = false;

    void Start()
    {
        cam = Camera.main;

        if (shootPoint == null)
        {
            Debug.LogError("Shoot point is not assigned.");
        }

        if (trajectoryRenderer == null)
        {
            Debug.LogError("Trajectory Renderer is not assigned.");
        }
        else
        {
            trajectoryRenderer.positionCount = 0;
        }

        // Instantiate the main ball at the shoot point
        mainBall = Instantiate(ballPrefab, shootPoint.position, Quaternion.identity);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnDragStart();
        }
        else if (Input.GetMouseButton(0))
        {
            OnDrag();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnDragEnd();
        }
    }

    void OnDragStart()
    {
        startPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        isDragging = true;
    }

    void OnDrag()
    {
        if (!isDragging) return;

        Vector3 currentPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        Vector3 direction = startPos - currentPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        mainBall.transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));

        // Draw the trajectory from the main ball's position
        DrawTrajectory(mainBall.transform.position, direction * shootForce);
    }

    void OnDragEnd()
    {
        if (!isDragging) return;

        endPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        Vector3 direction = startPos - endPos;
        Rigidbody rb = mainBall.GetComponent<Rigidbody>();
        rb.velocity = direction.normalized * shootForce;

        // Clear the trajectory
        ClearTrajectory();

        // Re-instantiate the main ball at the shoot point
        mainBall = Instantiate(ballPrefab, shootPoint.position, Quaternion.identity);
        isDragging = false;
    }

    void DrawTrajectory(Vector3 startPoint, Vector3 velocity)
    {
        if (trajectoryRenderer == null) return;

        float timeStep = 0.1f;
        int numPoints = 30;
        Vector3[] points = new Vector3[numPoints];
        trajectoryRenderer.positionCount = numPoints;

        for (int i = 0; i < numPoints; i++)
        {
            float t = i * timeStep;
            points[i] = startPoint + (velocity * t) + 0.5f * Physics.gravity * t * t;
        }

        trajectoryRenderer.SetPositions(points);
    }

    void ClearTrajectory()
    {
        if (trajectoryRenderer == null) return;
        trajectoryRenderer.positionCount = 0;
    }
}
